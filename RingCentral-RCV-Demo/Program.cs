using System;
using System.Threading.Tasks;
using RingCentral;
using dotenv.net;

namespace RingCentral_RCV_Demo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            DotEnv.Load();
            var rc = new RestClient(Environment.GetEnvironmentVariable("RINGCENTRAL_CLIENT_ID"),
                Environment.GetEnvironmentVariable("RINGCENTRAL_CLIENT_SECRET"),
                Environment.GetEnvironmentVariable("RINGCENTRAL_SERVER_URL"));
            await rc.Authorize(Environment.GetEnvironmentVariable("RINGCENTRAL_JWT_TOKEN"));

            // create a bridge
            var bridge = await rc.Rcvideo().V2().Account("~").Extension("~").Bridges().Post(new CreateBridgeRequest
            {
                name = "Test Meeting"
            });
            // delete the bridge after testing
            await rc.Rcvideo().V2().Bridges(bridge.id).Delete();
            
            
            // personal meeting
            var personalMeeting = await rc.Rcvideo().V2().Account("~").Extension("~").Bridges().Default().Get();
            Console.WriteLine($"You personal meeting URL is {personalMeeting.discovery.web}");

            // meeting history
            var r = await rc.Rcvideo().V1().History().Meetings().List();
            foreach(var meeting in r.meetings)
            {
                Console.WriteLine("Meeting:");
                Console.WriteLine($"  name: {meeting.displayName}");
                Console.WriteLine($"  start time: {meeting.startTime}");
                Console.WriteLine($"  duration: {meeting.duration}");
            }
        }
    }
}