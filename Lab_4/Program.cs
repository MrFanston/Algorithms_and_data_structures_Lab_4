using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab_4
{
    internal class Program
    {
        public class rent
        {
            private int price;
            private bool close;
            private string frequency_payment;
            private string name;
            private int number;

            public rent(int price_, string frequency_payment_, string name_, int number_)
            {
                price = price_;
                frequency_payment = frequency_payment_;
                name = name_;
                number = number_;
                close = false;
            }
            public int get_price()
            {
                return price;
            }
            public bool get_close()
            {
                return close;
            }
            public void set_close()
            {
                close = true;
            }
            public string get_frequency_payment()
            {
                return frequency_payment;
            }
            public string get_name()
            {
                return name;
            }
            public int get_number()
            {
                return number;
            }

        }
        static int BinarySearch(List<rent> list, int key, int left, int right) // бинарный поиск
        {
            int mid = left + (right - left) / 2; // находим середину вычитая из последнего элемента первый и деля на 2
            if (left >= right) // проверка условия, если левая сторона больше правой, то возвращается значение
                return -(1 + left);

            if (list[mid].get_number() == key) // если оказалось. что середина равна искомому значнию, то возвращается это значение, и поиск завершается
                return mid;

            else if (list[mid].get_number() > key)// в противном случае если середина больше искомого значения, то возвращаемся к левой части и продолжаем там алгоритм
                return BinarySearch(list, key, left, mid);
            else
                return BinarySearch(list, key, mid + 1, right);// иначе, если середина меньше искомого значения, то продолжаем поиск в правой части, так же деля массив на две части
        }
        static void Main(string[] args)
        {
            List<rent> list = new List<rent>();
            bool quit = true;
            int n;
            while (quit)
            {
                Console.WriteLine("Меню:\n1)Заключить новый договор\n2)Закрыть договор\n3)Найти договоры по помещению");
                Console.Write("Выбранный пункт: ");
                n = Convert.ToInt32(Console.ReadLine());
                string name;
                int price;
                string frequency_payment;
                int number;
                switch (n)
                {
                    case 1:
                        Console.WriteLine();
                        Console.Write("Имя арендатора: ");
                        name = Console.ReadLine();
                        Console.Write("Величина арендной платы: ");
                        price = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Периодичность платы: ");
                        frequency_payment = Console.ReadLine();
                        Console.Write("Номер помещения: ");
                        number = Convert.ToInt32(Console.ReadLine());
                        if (number < 1 ||  number > 300)
                        {
                            Console.WriteLine("Помещения с таким номером нет");
                            break;
                        }
                        bool no_rent = true;
                        for(int i = 0; i < list.Count(); i++)
                        {
                            // Если номер помещения совпадает с нужным
                            if (list[i].get_number() == number)
                            {
                                no_rent = false;
                                // Если договор не закрыт
                                if (!list[i].get_close())
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("Помещение уже арендуется");
                                    Console.WriteLine();
                                    break;
                                }   
                                // Если этот договор закрыт
                                else
                                {
                                    bool flag = true;
                                    // И все другие договоры с этим помещением закрыты
                                    for (int j = i + 1; j < list.Count(); j++)
                                    {
                                        if(!list[j].get_close() && list[j].get_number() == number)
                                        {
                                            Console.WriteLine();
                                            Console.WriteLine("Помещение уже арендуется");
                                            Console.WriteLine();
                                            flag = false;
                                            break;
                                        }
                                    }
                                    // Тогда создаем новый договор
                                    if(flag)
                                    {
                                        list.Add(new rent(price, frequency_payment, name, number));
                                        Console.WriteLine();
                                        Console.WriteLine("Договор записан");
                                        Console.WriteLine();
                                        break;
                                    }
                                }
                            }
                        }
                        if (no_rent)
                        {
                            list.Add(new rent(price, frequency_payment, name, number));
                            Console.WriteLine();
                            Console.WriteLine("Договор записан");
                            Console.WriteLine();
                            break;
                        }
                        break;
                    case 2:
                        Console.WriteLine();
                        Console.Write("Номер помещения: ");
                        number = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine();
                        for (int i = 0; i < list.Count(); i++)
                        {
                            // Если номер помещения совпадает с нужным
                            if (list[i].get_number() == number)
                            {
                                // Если договор не закрыт
                                if (!list[i].get_close())
                                {
                                    list[i].set_close();
                                    Console.WriteLine("Договор закрыт");
                                    Console.WriteLine();
                                    break;
                                }
                                else
                                {
                                    bool flag = true;
                                    // И все другие договоры с этим помещением закрыты
                                    for (int j = i + 1; j < list.Count(); j++)
                                    {
                                        if (!list[j].get_close() && list[j].get_number() == number)
                                        {
                                            Console.WriteLine();
                                            list[j].set_close();
                                            Console.WriteLine("Договор закрыт");
                                            Console.WriteLine();
                                            flag = false;
                                            break;
                                        }
                                    }
                                    // Тогда договор уже закрыт
                                    if (flag)
                                    {
                                        Console.WriteLine("Договор уже закрыт");
                                        Console.WriteLine();
                                        break;
                                    }
                                    break;
                                }
                            }
                        }
                        break;
                    case 3:
                        Console.WriteLine();
                        Console.Write("Номер помещения: ");
                        number = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine();
                        List<rent> list1 = new List<rent>();
                        list1.AddRange(list);

                        // Сортировка таблицы с договорами
                        for (int i = 0; i < list1.Count; i++)
                        {
                            for (int j = i + 1; j < list1.Count; j++)
                            {
                                if (list[i].get_number() > list[j].get_number())
                                {
                                    var temp = list1[i];
                                    list1[i] = list1[j];
                                    list1[j] = temp;
                                }
                            }
                        }
                        int count = BinarySearch(list1, number, 0, list1.Count);
                        if (count < 0)
                        {
                            Console.WriteLine("Договоры не найдены");
                            Console.WriteLine();
                            break;
                        }
                        while(true)
                        {
                            if(count > 0)
                            {
                                if (list1[count - 1].get_number() == number)
                                {
                                    count--;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        Console.WriteLine("Договоры:");
                        Console.WriteLine();
                        while (true)
                        {
                            if (count < list1.Count && list1[count].get_number() == number)
                            {
                                Console.Write("Имя арендатора: ");
                                Console.WriteLine(list1[count].get_name());
                                Console.Write("Величина арендной платы: ");
                                Console.WriteLine(list1[count].get_price());
                                Console.Write("Периодичность платы: ");
                                Console.WriteLine(list1[count].get_frequency_payment());
                                Console.Write("Номер помещения: ");
                                Console.WriteLine(list1[count].get_number());
                                if (list1[count].get_close())
                                {
                                    Console.WriteLine("Договор закрыт");
                                }
                                else
                                {
                                    Console.WriteLine("Договор действует");
                                }    
                                Console.WriteLine();
                                count++;
                            }
                            else
                            {
                                break;
                            }
                            
                        }
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("Некорректный пункт");
                        Console.WriteLine();
                        break;
                }
            }
        }
    }
}
