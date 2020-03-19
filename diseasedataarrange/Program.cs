using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diseasedataarrange
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime beginTime = DateTime.Now;
            Console.WriteLine("begin time:{0}", beginTime.ToString(@"yyyy-MM-dd HH:mm:ss.fff"));

            try
            {
                DdrMain main = new DdrMain();
                if (!main.DoWork(args))
                {
                    return;
                }

                TimeSpan oTime = DateTime.Now.Subtract(beginTime); 

                Console.WriteLine("time cost:{0}", oTime.ToString(@"hh\:mm\:ss\:fff"));

            }
            catch (Exception e)
            {
                Console.WriteLine("error:{0}", e.Message);
#if DEBUG
                Console.WriteLine("detail:{0}", e.ToString());
                Console.Read();
#endif
            }

            Console.WriteLine("end time:{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }
    }
}
