using System;
using System.IO;
namespace calculate
{
    internal class Program
    {
        static void Main(string[] args)
        {
            static bool JeZadanoCislo(string a)//funkce urcuje jestli bylo zadano cislo
            {
                double b;
                bool bo;
                bo = double.TryParse(a, out b);
                return bo;
            }
            static double Pamet(string a, double b, double c)//funkce pro ukladani cisel do pameti
            {
                if (a == "M+")
                {
                    return b + c;
                }
                else
                {
                    return -b + c;
                }
            }

            string Operace(string a)//funkce pro overeni zda uzivatel zadal platny operator
            {
                while (a != "+" & a != "-" & a != "*" & a != "/" & a != "!" & a != "^" & a != "M-" & a != "M+" & a != "C" & a != "MC" & a != "M")
                {
                    Console.WriteLine("zadejte prosim jeden z operatoru: +, -, *, /, !, ^, M-, M+, MC, C");
                    a = Console.ReadLine();

                }
                return a;
            }

            using (FileStream stream = new FileStream("zurnal.txt", FileMode.OpenOrCreate)) ;//vytvareni nebo otrvreni souboru pro ukladani historie vypoctu
            string operace;//zavedeni promenne pro urceni operace
            double m;//promenna se kterou se budeme pocitat
            double pamet = 0;//promenna pro pamet, 
        Zacatek:// sem se budeme vracet po vycisteni
            Console.WriteLine("Kalkulacka umi operace: +, -, *, /, !, ^, M-, M+, MC\nPro vycisteni zadejte C\n" +
                "Pro vyuziti cisla z pameti zadejte MR\nPokud chcete se na tuto hodnotu podivat, zadejte M");
            string test = Console.ReadLine();
            double cislo = 0;

            if (test == "MR")
            {
                cislo = pamet;
                Console.WriteLine("M = " + cislo + "\n" + cislo);
                test = Console.ReadLine();
            }
            else if (JeZadanoCislo(test))
            {
                bool b;
                double c;
                b = double.TryParse(test, out c);
                while (b)
                {
                    cislo = c;
                    test = Console.ReadLine();
                    b = double.TryParse(test, out c);
                    if (test == "MR")
                    {
                        cislo = pamet;
                        Console.WriteLine("M = " + cislo + "\n" + cislo);
                        test = Console.ReadLine();
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Zadejte prosim cislo");
                goto Zacatek;
            }//na zacatku ocekavame cislo, pokud uzivatel zada nekolik cisel za sebou, budeme se pocitat s tim poslednim, nebo vola pamet
             //na zacatku pamet je nula, ale pokud se sem dostalo po vycisteni konzole, nejaka hodnota v pameti byt muze

            m = cislo;


            operace = Operace(test);

            goto Metka1;//mame tady zadanou operace, pokud bycho sli dale, nacetla be se do nej nova hodnota, potrebujeme tuto cast preskocit

        Pokracovani://sem se vratime pe dokonceni nejake operace

            string pokus = Console.ReadLine();
            if (JeZadanoCislo(pokus))
            {
                m = double.Parse(pokus);
                goto Pokracovani;
            }
            else if (pokus == "+" || pokus == "-" || pokus == "*" || pokus == "/" || pokus == "!" || pokus == "^" ||
                pokus == "M-" || pokus == "M+" || pokus == "C" || pokus == "MC" || pokus == "M")
            {
                operace = Operace(pokus);
            }
            else if (pokus == "MR")
            {
                m = pamet;
                Console.WriteLine("M = " + m + "\n" + m);
                goto Pokracovani;
            }
            else
            {
                Console.WriteLine("Zadejte cislo nebo operator");
                goto Pokracovani;
            }// po dokonceni jedne matematicke operace je mozne pokracovat pracovat s vysledkem nebo zadat nove cislo,
             // rozhodneme co uzivatel chce podle toho co zadal

        Metka1:
            string str1 = Convert.ToString(m);//promenna je potreba pro zapis historie, pocatecni hodnota m
            if (operace == "C")//preruseni vypoctu
            {
                Console.Clear();
                goto Zacatek;
            }
            else if (operace == "M")
            {
                Console.WriteLine("M = " + pamet);
            }

            else if (operace == "MC")
            {
                pamet = 0;
                Console.WriteLine("M = " + pamet);
            }
            else if (operace == "M-" || operace == "M+")
            {
                pamet = Pamet(operace, m, pamet);
                Console.WriteLine("M = " + pamet);
                Console.WriteLine(m);
            }
            else if (operace == "!")
            {
                double n = m;
                for (int i = 1; i < m; i++)
                {
                    n = n * i;
                }//vypocet faktorialu
                m = n;
                using (FileStream stream = new FileStream("zurnal.txt", FileMode.Append))//otevreni souboru pro zapis
                {
                    string str2 = Convert.ToString(m);
                    byte[] buffer = System.Text.Encoding.Default.GetBytes(str1 + operace + "=" + str2 + "\n");
                    stream.Write(buffer, 0, buffer.Length);//zapis provedene operace do souboru
                }

            }
            else//ostatni operace
            {
                double c2;//pro operace uvedene nize uy potrebujeme druhe cislo, muze to byt nova hodnota nebo hodnota kterou kalkulacka pamatuje
                string str = Console.ReadLine();
                if (str == "C")
                {
                    Console.Clear();
                    goto Zacatek;
                }
                else if (str == "MR")
                {
                    c2 = pamet;
                }
                else//pokud nechce vyuzit pamet overinme ze zadava opravdu cislo
                {

                    bool bo;
                    do
                    {
                        bo = double.TryParse(str, out c2);
                        if (bo == false)
                        {
                            Console.WriteLine("zadejte prosim cislo");
                            str = Console.ReadLine();
                            if (str == "MR")//pokud rozhodne vyuzit pameti
                            {
                                bo = true;
                                c2 = pamet;
                                Console.WriteLine("M = " + c2);
                            }
                            else if (str == "C")//nebo rozhodne ukoncit vypocet
                            {
                                Console.Clear();
                                goto Zacatek;
                            }
                        }

                    } while (bo == false);
                }
                switch (operace)//jednoduche operace
                {

                    case "+":
                        m = m + c2;
                        break;
                    case "-":
                        m = m - c2;
                        break;
                    case "*":
                        m = m * c2;
                        break;
                    case "/":
                        if (c2 == 0)
                            {
                            Console.WriteLine("error");
                        }
                            else
                        {
                            m = m / c2;
                        }
                        break;
                    case "^":
                        m = Math.Pow(m, c2);
                        break;

                }
                string str3 = Convert.ToString(m);//promenna pro zapis do zurnalu
                using (FileStream stream = new FileStream("zurnal.txt", FileMode.Append))//otevreni souboru pro zapis
                {
                    string str2 = Convert.ToString(c2);
                    byte[] buffer = System.Text.Encoding.Default.GetBytes(str1 + operace + str2 + "=" + str3 + "\n");
                    stream.Write(buffer, 0, buffer.Length);//zapiseme text do souboru
                }

            }

            string[] st = File.ReadAllLines("zurnal.txt");
            string s = st[st.Length - 1];
            Console.WriteLine(s);//vypiseme provedenou operace a vysledek ze souboru, do ktereho to bylo zapsano

            goto Pokracovani;// pokracujeme ve vypoctu



        }
    }
}
