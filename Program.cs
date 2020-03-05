using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace Hello
{
    class Program
    {

        //this is the int list used through out the applicaiton
        private static List<int> _Listint = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        //List of reasons
        private static List<string> Reasons = new List<string>() { "",""
            ,"I dev to.."
            ," thrive in great culture"
            ,"  grow with great technology"
            ,"   share great experiences with peers"
            ,"    adapt and aspire to meet greater quality\n\n"
        };


        /// <summary>
        /// Main control: Shows a simple list of commands that the user can than run by typing in a command.  
        /// Each command is different and works a different area of technology.
        /// Does capture and report any errors.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //_input: is used as the command menu parameter
            string _input = string.Empty;
            do
            {
                Console.WriteLine("List of commands.  Type in a command from the list shown. ");
                Console.WriteLine("Type \"star\": I dev to..");
                Console.WriteLine("Type \"c1\": Sum all even numbers in a list.");
                Console.WriteLine("Type \"c2\": Make a http GET request to a URL and display the results.");
                Console.WriteLine("Type \"c3\": Print out a list of numbers in a loop with a configurable delay.  Simple thread.");
                Console.WriteLine("Type \"c4\": Print one list every 500ms and another asynchronously every 1000ms.  Threads.");
                Console.WriteLine("Type \"c5\": Call a http service and display the results.");

                Console.WriteLine("Type \"clear\": to clear screen");
                Console.WriteLine("Type \"exit\": to exit the console application.");
                _input = Console.ReadLine();

                switch (_input.ToLower())
                {
                    case "star":
                        Console.Clear();
                        foreach (string item in Reasons)
                            Console.WriteLine(item);
                        Console.WriteLine("");
                        Console.WriteLine("Type any key to continue.");
                        Console.ReadLine();
                        break;
                    case "c1":
                        Console.Clear();
                        Console.WriteLine("Sum is " + SumEven(_Listint).ToString() + ".  Type any key to continue.");
                        Console.ReadLine();
                        break;
                    case "c2":
                        Console.Clear();
                        c2();
                        Console.WriteLine("Type any key to continue.");
                        Console.ReadLine();
                        break;
                    case "c3":
                        Console.Clear();
                        c3(_Listint);
                        Console.WriteLine("Type any key to continue.");
                        Console.ReadLine();
                        break;
                    case "c4":
                        Console.Clear();
                        c4();
                        Console.WriteLine("Type any key to continue.");
                        Console.ReadLine();
                        break;
                    case "c5":
                        Console.Clear();
                        c5();
                        Console.WriteLine("Type any key to continue.");
                        Console.ReadLine();
                        break;
                    case "c6":
                        Console.Clear();
                        //c6();
                        Console.WriteLine("Type any key to continue.");
                        Console.ReadLine();
                        break;
                    case "clear":
                        Console.Clear();
                        break;
                    case "exit":
                        break;
                    default:
                        Console.WriteLine("Command not recognized.");
                        break;
                }

            } while (_input != "exit");

            Environment.Exit(0);
        }


        /// <summary>
        /// Call a rest service and display the results
        /// </summary>
        private static void c5()
        {
            string url = "http://worldtimeapi.org/api/ip";
            Console.WriteLine("Directions: type return to use the url rest service (" + url + ") or type in the service url.");
            string _sL = Console.ReadLine();
            if (!string.IsNullOrEmpty(_sL))
            {
                //todo: test if the string is a service url.
                url = _sL;
            }

            try
            {

                HttpClient http = new HttpClient();
                HttpResponseMessage response = http.GetAsync(new Uri(url)).Result;
                string responseBody = string.Empty;

                //Testing response and rerouting to the appropriate action
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        responseBody = response.Content.ReadAsStringAsync().Result;
                        Console.WriteLine(responseBody);
                        break;
                    case HttpStatusCode.InternalServerError:
                    case HttpStatusCode.RequestTimeout:
                    default:
                        Console.WriteLine(response.StatusCode);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }
        }



        /// <summary>
        /// One thread should output at 500ms
        /// Second thread should output at 1000ms
        /// Wait until all threads are complete.
        /// Sample output should be
        /// t1: 1
        /// t2: 5
        /// t1: 2
        /// t1: 3
        /// t2: 4
        /// t1: 4
        /// t1: 5
        /// t2: 3
        /// t2: 2
        /// t2: 1
        /// </summary>
        private static void c4()
        {
            List<int> _suppList = new List<int> { 1, 2, 3, 4, 5 };
            Console.WriteLine("Directions: type return to use the supplied list of (" + string.Join(",", _suppList.Select(n => n.ToString()).ToArray()) + ") or type in a comma delimited list.");
            string _sL = Console.ReadLine();
            if (!string.IsNullOrEmpty(_sL))
            {
                //todo: test if the string is not a cdl of ints.
                _suppList = _sL.Split(',').Select(Int32.Parse).ToList();
            }

            //run parallel threads (x2) asynchronous
            //Wait until both are complete

            Thread newThreadAsc = new Thread(delegate () { DoWorkAsc(_suppList, "t1", 500); });
            Thread newThreadDesc = new Thread(delegate () { DoWorkDesc(_suppList, "t2", 1000); });

            newThreadAsc.Start();
            newThreadDesc.Start();

            newThreadAsc.Join();
            newThreadDesc.Join();

        }
        private static void DoWorkAsc(List<int> _suppList, string _Thread, int _Sleep)
        {
            for (int i = 0; i < _suppList.Count; i++)
            {

                Console.WriteLine(_Thread + ": " + _suppList[i].ToString());
                Thread.Sleep(_Sleep);

            }
        }
        private static void DoWorkDesc(List<int> _suppList, string _Thread, int _Sleep)
        {
            for (int j = _suppList.Count; j > 0; j--)
            {

                Console.WriteLine(_Thread + ": " + _suppList[j - 1].ToString());
                Thread.Sleep(_Sleep);

            }
        }

        /// <summary>
        /// Print out a list with a configurable delay.  
        /// Thread. 
        /// </summary>
        private static void c3(List<int> _suppList)
        {
            Console.WriteLine("Directions 1/2: type return to use this list (" + string.Join(",", _suppList.Select(n => n.ToString()).ToArray()) + ") or type in a comma delimited list.");
            string _sL = Console.ReadLine();
            if (!string.IsNullOrEmpty(_sL))
            {
                //todo: test if the string is not a cdl of ints.
                _suppList = _sL.Split(',').Select(Int32.Parse).ToList();
            }

            Console.WriteLine("Directions 2/2: type return to set the delay to (200ms) or type in the delay.  Input format for the delay is a whole number, via.. 100, 300, etc.");
            dynamic _delay = Console.ReadLine();
            if (string.IsNullOrEmpty(_delay))
            {
                _delay = "200";
            }

            //todo: test if is an int
            _delay = int.Parse(_delay);

            foreach (int _num in _suppList)
            {
                Thread newThread = new Thread(DoWork);
                newThread.Start(_num);
                Thread.Sleep(_delay);
            }
        }

        private static void DoWork(object obj)
        {
            Console.WriteLine(obj.ToString());
        }

        /// <summary>
        /// Make a http GET request to a URL and display the results. 
        /// </summary>
        private static void c2()
        {

            string html = string.Empty;
            string url = @"https://www.wikipedia.org/";
            Console.WriteLine("Directions: type return to use (" + url + ") or type in a URL.");
            string _sL = Console.ReadLine();
            if (!string.IsNullOrEmpty(_sL))
            {
                //todo: test if input is not of a working url.
                url = _sL;
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            Console.WriteLine(html);
        }

        /// <summary>
        /// Sum up all the even numbers in a list and return the result.
        /// </summary>
        /// <param name="_suppList"></param>
        /// <returns>int</returns>
        private static int SumEven(List<int> _suppList)
        {
            Console.WriteLine("Directions: type return to use the list of (" + string.Join(",", _suppList.Select(n => n.ToString()).ToArray()) + ") or type in a comma delimited list.");
            string _sL = Console.ReadLine();
            if (!string.IsNullOrEmpty(_sL))
            {
                //todo: test if the string is not a cdl of ints.
                _suppList = _sL.Split(',').Select(Int32.Parse).ToList();
            }
            int _sum = 0;
            foreach (int _num in _suppList)
            {
                if (_num % 2 == 0)
                    _sum += _num;
            }
            return _sum;
        }
    }
}

