using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Skeletonization.BusinessLayer.Abstractions;
using Skeletonization.CrossfulLayer.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Skeletonization.BusinessLayer.Implementation.Detection
{
    public class Detector : IDetector
    {
        private const int nPoints = 18;
        private readonly Net _net;

        private static (int first, int second)[] mapIdx =
        {
            (31,32), (39,40), (33,34), (35,36), (41,42), (43,44),
            (19,20), (21,22), (23,24), (25,26), (27,28), (29,30),
            (47,48), (49,50), (53,54), (51,52), (55,56), (37,38),
            (45,46)
        };

        private static (int first, int second)[] posePairs =
        {
            (1,2), (1,5), (2,3), (3,4), (5,6), (6,7),
            (1,8), (8,9), (9,10), (1,11), (11,12), (12,13),
            (1,0), (0,14), (14,16), (0,15), (15,17), (2,17),
            (5,16)
        };

        public Detector(Net net)
        {
            _net = net;
        }

        private static void GetKeyPoints(Mat probMap, double threshold, out KeyPoint[] keyPointsResult)
        {
            var smoothProbMap = new Mat();
            CvInvoke.GaussianBlur(probMap, smoothProbMap, new Size(3, 3), 0, 0);

            var maskedProbMap = new Mat();
            CvInvoke.Threshold(smoothProbMap, maskedProbMap, threshold, 255, ThresholdType.Binary);

            maskedProbMap.ConvertTo(maskedProbMap, DepthType.Cv8U, 1);

            var contours = new VectorOfVectorOfPoint();
            var empty = new Mat();
            CvInvoke.FindContours(maskedProbMap, contours, empty, RetrType.Tree, ChainApproxMethod.ChainApproxSimple);

            var keyPoints = new List<KeyPoint>();

            for (int i = 0; i < contours.Size; ++i)
            {
                var blobMask = Mat.Zeros(smoothProbMap.Rows, smoothProbMap.Cols, smoothProbMap.Depth, smoothProbMap.NumberOfChannels);

                CvInvoke.FillConvexPoly(blobMask, contours[i], new MCvScalar(1));
                double _ = 0;
                double __ = 0;
                Point ___ = default;
                Point maxLoc = default;

                var mult = new Mat();
                CvInvoke.Multiply(smoothProbMap, blobMask, mult);
                CvInvoke.MinMaxLoc(mult, ref _, ref __, ref ___, ref maxLoc);

                keyPoints.Add(new KeyPoint(maxLoc, probMap.At<float>(maxLoc.Y, maxLoc.X)));
            }

            keyPointsResult = keyPoints.ToArray();
        }

        private static void SplitNetOutputBlobToParts(Mat netOutputBlob, Size targetSize, out Mat[] netOutputsPartsResult)
        {
            int nparts = netOutputBlob.SizeOfDimension[1];
            int h = netOutputBlob.SizeOfDimension[2];
            int w = netOutputBlob.SizeOfDimension[3];

            var netOutputsParts = new List<Mat>();
            for (int i = 0; i < nparts; ++i)
            {
                var part = new Mat(new[] { h, w }, DepthType.Cv32F, netOutputBlob.GetDataPointer(0, i));
                var resizedMat = new Mat();
                CvInvoke.Resize(part, resizedMat, targetSize);
                netOutputsParts.Add(resizedMat);
            }

            netOutputsPartsResult = netOutputsParts.ToArray();
        }

        private static void PopulateInterpPoints(Point a, Point b, int numPoints, out Point[] interpCoordsResult)
        {

            var interpCoords = new List<Point>();

            float xStep = (b.X - a.X) / (float)(numPoints - 1);
            float yStep = (b.Y - a.Y) / (float)(numPoints - 1);

            interpCoords.Add(a);

            for (int i = 1; i < numPoints - 1; ++i)
            {
                interpCoords.Add(new Point((int)(a.X + xStep * i), (int)(a.Y + yStep * i)));
            }

            interpCoords.Add(b);

            interpCoordsResult = interpCoords.ToArray();
        }

        private static void GetValidPairs(Mat[] netOutputParts,
                                  KeyPoint[][] detectedKeyPoints,
                                  out ValidPair[][] validPairsResult,
                                  out int[] invalidPairsResult)
        {
            int nInterpSamples = 10;
            float pafScoreTh = 0.1f;
            float confTh = 0.7f;

            var validPairs = new List<List<ValidPair>>();
            var invalidPairs = new SortedSet<int>();

            for (int k = 0; k < mapIdx.Length; ++k)
            {
                var pafA = netOutputParts[mapIdx[k].first];
                var pafB = netOutputParts[mapIdx[k].second];

                var candA = detectedKeyPoints[posePairs[k].first];
                var candB = detectedKeyPoints[posePairs[k].second];

                int nA = candA.Length;
                int nB = candB.Length;

                if (nA != 0 && nB != 0)
                {
                    var localValidPairs = new List<ValidPair>();

                    for (int i = 0; i < nA; ++i)
                    {
                        int maxJ = -1;
                        float maxScore = -1;
                        bool found = false;

                        for (int j = 0; j < nB; ++j)
                        {
                            (float first, float second) = (candB[j].Point.X - candA[i].Point.X, candB[j].Point.Y - candA[i].Point.Y);
                            float norm = MathF.Sqrt(first * first + second * second);

                            if (norm == 0)
                            {
                                continue;
                            }

                            first /= norm;
                            second /= norm;

                            PopulateInterpPoints(candA[i].Point, candB[j].Point, nInterpSamples, out var interpCoords);

                            var pafInterp = new List<(float first, float second)>();

                            for (int l = 0; l < interpCoords.Length; ++l)
                            {
                                pafInterp.Add
                                    ((pafA.At<float>(interpCoords[l].Y, interpCoords[l].X),
                                      pafB.At<float>(interpCoords[l].Y, interpCoords[l].X)));
                            }

                            var pafScores = new List<float>();
                            float sumOfPafScores = 0;
                            int numOverTh = 0;
                            for (int l = 0; l < pafInterp.Count; ++l)
                            {
                                float score = pafInterp[l].first * first + pafInterp[l].second * second;
                                sumOfPafScores += score;
                                if (score > pafScoreTh)
                                {
                                    ++numOverTh;
                                }

                                pafScores.Add(score);
                            }

                            float avgPafScore = sumOfPafScores / pafInterp.Count;

                            if (numOverTh / (float)nInterpSamples > confTh)
                            {
                                if (avgPafScore > maxScore)
                                {
                                    maxJ = j;
                                    maxScore = avgPafScore;
                                    found = true;
                                }
                            }
                        }//j

                        if (found)
                        {
                            localValidPairs.Add(new ValidPair(candA[i].Id, candB[maxJ].Id, maxScore));
                        }
                    }//i

                    validPairs.Add(localValidPairs);
                }
                else
                {
                    invalidPairs.Add(k);
                    validPairs.Add(new List<ValidPair>());
                }
            }//k

            validPairsResult = validPairs.Select(x => x.ToArray()).ToArray();
            invalidPairsResult = invalidPairs.ToArray();
        }

        private static void GetPersonwiseKeypoints(ValidPair[][] validPairs,
                                           int[] invalidPairs,
                                           out int[][] personwiseKeypointsResult)
        {
            var personwiseKeypoints = new List<List<int>>();

            for (int k = 0; k < mapIdx.Length; ++k)
            {
                if (invalidPairs.Contains(k))
                {
                    continue;
                }

                var localValidPairs = new List<ValidPair>(validPairs[k]);

                int indexA = posePairs[k].first;
                int indexB = posePairs[k].second;

                for (int i = 0; i < localValidPairs.Count; ++i)
                {
                    bool found = false;
                    int personIdx = -1;

                    for (int j = 0; !found && j < personwiseKeypoints.Count; ++j)
                    {
                        if (indexA < personwiseKeypoints[j].Count &&
                           personwiseKeypoints[j][indexA] == localValidPairs[i].Aid)
                        {
                            personIdx = j;
                            found = true;
                        }
                    }

                    if (found)
                    {
                        personwiseKeypoints[personIdx][indexB] = localValidPairs[i].Bid;
                    }
                    else if (k < 17)
                    {
                        var lpkp = new List<int>(Enumerable.Range(0, 18).Select(_ => -1))
                        {
                            [indexA] = localValidPairs[i].Aid,
                            [indexB] = localValidPairs[i].Bid
                        };

                        personwiseKeypoints.Add(lpkp);
                    }
                }
            }

            personwiseKeypointsResult = personwiseKeypoints.Select(x => x.ToArray()).ToArray();
        }

        public Point[,] Detect(Mat input)
        {
            var inputBlob = DnnInvoke.BlobFromImage(input, 1.0 / 255.0, new Size(368 * input.Cols / input.Rows, 368), new MCvScalar(0, 0, 0), false, false);
            _net.SetInput(inputBlob);

            var netOutputBlob = _net.Forward();
            SplitNetOutputBlobToParts(netOutputBlob, new Size(input.Cols, input.Rows), out var netOutputParts);

            int keyPointId = 0;
            var detectedKeypoints = new KeyPoint[nPoints][];
            var keyPointsList = new List<KeyPoint>();

            for (int i = 0; i < nPoints; ++i)
            {
                GetKeyPoints(netOutputParts[i], 0.1, out var keyPoints);
                for (int j = 0; j < keyPoints.Length; j++)
                {
                    keyPoints[j].Id = keyPointId++;
                }

                detectedKeypoints[i] = keyPoints;
                keyPointsList.AddRange(keyPoints);
            }

            GetValidPairs(netOutputParts, detectedKeypoints, out var validPairs, out int[] invalidPairs);
            GetPersonwiseKeypoints(validPairs, invalidPairs, out int[][] personwiseKeypoints);

            var persons = new Point[personwiseKeypoints.Length, nPoints];

            for (int i = 0; i < persons.GetLength(0); i++)
            {
                for (int j = 0; j < persons.GetLength(1); j++)
                {
                    persons[i, j] = new Point(-1, -1);
                }
            }

            for (int i = 0; i < nPoints - 1; ++i)
            {
                for (int n = 0; n < personwiseKeypoints.Length; ++n)
                {
                    var (first, second) = posePairs[i];
                    int indexA = personwiseKeypoints[n][first];
                    int indexB = personwiseKeypoints[n][second];

                    if (indexA == -1 || indexB == -1)
                    {
                        continue;
                    }

                    var kpA = keyPointsList[indexA];
                    var kpB = keyPointsList[indexB];

                    persons[n, first] = kpA.Point;
                    persons[n, second] = kpB.Point;
                }
            }


            return persons;
        }
    }
}