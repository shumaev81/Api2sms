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
				if (args.Length == 0)
                {
                    Console.WriteLine("Использование:   api2sms [API number message]");
                    Console.WriteLine("                         [API --balanse]");
                    Console.WriteLine("Параметры:");
                    Console.WriteLine("     API             ключ API на sms.ru");
                    Console.WriteLine("     number          номер телефона в формате 7XXXXXXXXXX");
                    Console.WriteLine("     message         сообщение, пробелы заменить на символ +");
                    Console.WriteLine("     --balanse       выводит баланс на sms.ru");
                    Console.WriteLine("");
                    System.Environment.Exit(1);
                }
				string API = args[0];
				if (args[0] == "shumaev") API = "0b3aa42c-d701-7064-e12d-3fd20a5d154b";
				if (args[1] == "--balanse")
                {
                    urlResponse = "https://sms.ru/my/balance?api_id=" + API + "&json=1";
                }
                else
                {
                    urlResponse = "https://sms.ru/sms/send?api_id=" + API + "&to=" + args[1] + "&msg=" + args[2] + "&json=1";
                }
                //Console.WriteLine("Строка HTTP-запроса:");
                //Console.WriteLine(urlResponse);
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
            //Console.WriteLine("Нажмите Enter...");
            //Console.Read();
        }
    }
}
