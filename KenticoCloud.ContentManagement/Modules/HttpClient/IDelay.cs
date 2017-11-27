using System;
using System.Threading.Tasks;

namespace KenticoCloud.ContentManagement.Modules.HttpClient
{
    internal interface IDelay
    {
        Task DelayByMs(TimeSpan delay);
    }
}
