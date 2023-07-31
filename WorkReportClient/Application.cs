using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkReportClient.Services;

namespace WorkReportClient
{
    public class Application
    {
        private readonly List<string> defaultMachineNumbers =
            new List<string> { "M1", "M2", "M3" };

        private readonly IWorkReportService _workReportService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="workReportService"></param>
        public Application(IWorkReportService workReportService) 
        {
            _workReportService = workReportService;
        }

        /// <summary>
        /// 程式起始點
        /// </summary>
        public void Run() 
        {
            _workReportService.Report(new ReportModel
            {
                MachineNumber = GetMachineNumber(),
                SpendTimeHour = GetSpendTimeHour(),
                SpendTimeMinute = GetSpendTimeMinute(),
                SpendTimeSecond = GetSpendTimeSecond(),
            });

        }

        /// <summary>
        /// 取得設備代碼
        /// </summary>
        /// <returns></returns>
        private string GetMachineNumber() 
        {
            string machineNumber = "";

            while (string.IsNullOrWhiteSpace(machineNumber))
            {
                Console.WriteLine("Please input machine number. Ex: M1, M2 or m3");
                machineNumber = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(machineNumber))
                {
                    Console.WriteLine("Cannot input space or null.");
                    machineNumber = "";
                }
                else if (!defaultMachineNumbers.Contains(machineNumber))
                {
                    Console.WriteLine("Invalid machine number");
                    machineNumber = "";
                }
            }

            return machineNumber;
        }

        /// <summary>
        /// 取得花費時間(小時)
        /// </summary>
        /// <returns></returns>
        private int GetSpendTimeHour()
        {
            bool isHaveValue = false;
            int spendTimeHour = 0;

            while (!isHaveValue)
            {
                Console.WriteLine("Please input spend time (hour).");
            
                if (!int.TryParse(Console.ReadLine(), out spendTimeHour)) 
                {
                    Console.WriteLine("Invalid input value");
                    continue;
                }

                if (spendTimeHour < 0)
                {
                    Console.WriteLine("The input value must be greater than or equal to zero.");
                    continue;
                }

                isHaveValue = true;
            }

            return spendTimeHour;
        }

        /// <summary>
        /// 取得花費時間(分)
        /// </summary>
        /// <returns></returns>
        private int GetSpendTimeMinute()
        {
            bool isHaveValue = false;
            int spendTimeMinute = 0;

            while (!isHaveValue)
            {
                Console.WriteLine("Please input spend time (minute).");

                if (!int.TryParse(Console.ReadLine(), out spendTimeMinute))
                {
                    Console.WriteLine("Invalid input value");
                    continue;
                }

                if (spendTimeMinute < 0)
                {
                    Console.WriteLine("The input value must be greater than or equal to zero.");
                    continue;
                }
                else if (spendTimeMinute > 60)
                {
                    Console.WriteLine("The input value must be less than or equal to 60.");
                    continue;
                }

                isHaveValue = true;
            }

            return spendTimeMinute;
        }

        /// <summary>
        /// 取得花費時間(秒)
        /// </summary>
        /// <returns></returns>
        private int GetSpendTimeSecond()
        {
            bool isHaveValue = false;
            int spendTimeSecond = 0;

            while (!isHaveValue)
            {
                Console.WriteLine("Please input spend time (second).");

                if (!int.TryParse(Console.ReadLine(), out spendTimeSecond))
                {
                    Console.WriteLine("Invalid input value");
                    continue;
                }

                if (spendTimeSecond < 0)
                {
                    Console.WriteLine("The input value must be greater than or equal to zero.");
                    continue;
                }
                else if (spendTimeSecond > 60)
                {
                    Console.WriteLine("The input value must be less than or equal to 60.");
                    continue;
                }

                isHaveValue = true;
            }

            return spendTimeSecond;
        }
    }
}
