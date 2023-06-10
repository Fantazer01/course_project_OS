﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace course_project_OS
{
    class CommandParams
    {
        public double Number1 { get; set; }
        public double Number2 { get; set; }
        public string Operation { get; set; } = string.Empty;
        public int Timer { get; set; } 
        public CommandParams() { }

    }

    internal class CalcProcessing
    {
        static readonly string introLine = "Для выполнения арифметической операции введите в указанном порядке:\n"
                                + "число1 число2 операцию(+,-,*,/) время_задержки\n"
                                + "И нажмите Enter";
        public static void CalcDialog()
        {
            Console.WriteLine(introLine);
            string? line = Console.ReadLine();
            if (line == null)
                return;


            CommandParams? commandParams;
            if (!TryParseParams(line, out commandParams))
                return;

            Console.WriteLine("Happy end!");
        }

        private static bool TryParseParams(string line, out CommandParams? commandParams)
        {
            commandParams = new CommandParams();
            string[] param = line.Split(' ');
            if (param.Length != 4)
            {
                Console.WriteLine("Ошибка. Некорректное количество параметров.");
                return false;
            }

            try
            {
                commandParams.Number1 = double.Parse(param[0]);
                commandParams.Number2 = double.Parse(param[1]);
                if (param[2] == "+" || param[2] == "-" || param[2] == "*" || param[2] == "/")
                    commandParams.Operation = param[2];
                else
                    throw new Exception("Incorrect operation parameter in command");
                commandParams.Timer = int.Parse(param[3]);
                
            } catch
            {
                Console.WriteLine("Ошибка. Некорректный формат одного из параметров.");
                return false;
            }
            
            
            return true;
        }
    }
}
