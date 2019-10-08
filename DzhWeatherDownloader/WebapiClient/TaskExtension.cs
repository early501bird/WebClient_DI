using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DzhWeatherDownloader.WebapiClient
{
    public static class TaskExtension
    {
        public static T WaitForResult<T>(this Task<T> task)
        {
            task.Wait();
            return task.Result;
        }
    }
}
