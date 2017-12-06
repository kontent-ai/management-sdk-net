using System;
using System.Threading.Tasks;

namespace KenticoCloud.ContentManagement.Modules.HttpClient
{
    internal interface IDelay
    {
        Task DelayByTimeSpan(TimeSpan delay);
    }
}
