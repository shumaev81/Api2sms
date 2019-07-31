using System;
using System.Net;
using System.IO;

namespace Api2sms
{
    class Program
    {
        static void Main(string[] args)
        {
			Console.WriteLine("");
            try
            {
				string urlResponse = "https://sms.ru";
				//Если нет параметров выводим инфу и на выход
				if (args.Length == 0)
                {
                    Console.WriteLine("Использование:   api2sms [API number message]");
                    Console.WriteLine("                         [API -balance]");
                    Console.WriteLine("Параметры:");
                    Console.WriteLine("     API             ключ API на sms.ru");
                    Console.WriteLine("     number          номер телефона в формате 7XXXXXXXXXX");
                    Console.WriteLine("     message         сообщение, пробелы заменить на символ +");
                    Console.WriteLine("     -balance        выводит баланс на sms.ru");
                    Console.WriteLine("");
                    System.Environment.Exit(1);
                }
				string API = args[0];
				string nmbr = args[1];
				if (args[1] == "-balance")              //Если второй параметр -balance
				{
					urlResponse = "https://sms.ru/my/balance?api_id=" + API + "&json=1";
                }
				else									//иначе собираем строку запроса
				{
					string msg = args[2];
					urlResponse = "https://sms.ru/sms/send?api_id=" + API + "&to=" + nmbr + "&msg=" + msg + "&json=1";
                }
                //отправка HTTP-запроса
                WebRequest request = WebRequest.Create(urlResponse);
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
							string line = "";
                            while ((line = reader.ReadLine()) != null)
                            {
                                Console.WriteLine(line);
                            }
                        }
                    }
                    response.Close();
                    Console.WriteLine("Запрос выполнен.");
                }
            }
            catch (WebException exHttp)
            {
                Console.WriteLine("Ошибка HTTP-запроса:");
                // пишем текст ошибки
                Console.WriteLine(exHttp.ToString());
                // получаем статус исключения
                WebExceptionStatus status = exHttp.Status;

                if (status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)exHttp.Response;
                    Console.WriteLine("Статусный код ошибки: {0} - {1}", (int)httpResponse.StatusCode, httpResponse.StatusCode);
                }
            }
            catch (Exception e)
            {
                // вывод прочих ошибок
                Console.WriteLine("Неизвестная ошибка:");
                Console.WriteLine(e.ToString());
            }
        }
    }
}
