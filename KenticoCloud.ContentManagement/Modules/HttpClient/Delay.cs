using System;
using System.Threading.Tasks;

namespace KenticoCloud.ContentManagement.Modules.HttpClient
{
    internal class Delay: IDelay
    {
        public async Task DelayByTimeSpan(TimeSpan delay)
        {
            await Task.Delay(delay);
        }
    }
}
