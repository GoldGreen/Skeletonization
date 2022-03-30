using Skeletonization.CrossLayer.Data;
using System;

namespace Skeletonization.CrossLayer.Exceptions
{
    public class VideoCaptureFabricException : Exception
    {
        public VideoCaptureFabricException(VideoCaptureFactoryType videoCaptureFabricType)
            : base($"Неверная опция фабрики создания видеопотока: {videoCaptureFabricType}")
        {

        }
        public VideoCaptureFabricException(VideoCaptureFactoryType videoCaptureFabricType, object arg, Type excepted)
           : base($"Неверный тип аргумента для фабрики создания видеопотока: {videoCaptureFabricType}, аргумент: {arg} тип {arg?.GetType().Name}, ожидалось {excepted.Name}")
        {

        }
    }
}
