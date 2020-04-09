using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediiEgale
{
    class Program
    {
        static void Main(string[] args)
        {
            // (102 + 79 + 20) / 3 = (120 + 14) / 2 = 67
            List<double> list = new List<double> { 79, 20, 14, 120, 102 };
            Console.WriteLine(CanBeSplitIntoTwoEqualAverages(list));

            list = new List<double> { 10, 4, 2, 1, 3, 9 };
            Console.WriteLine(CanBeSplitIntoTwoEqualAverages(list));
            Console.ReadLine();
        }

        // Daca avem conditia ca medie(B) = medie(C)
        //  si medie(B) = SB / NB (suma / nr elem), medie(C) = SC / NC = (SA - SB) / (NA - NB)
        //  => dupa calcule si simplificari, medie(B) = medie(A)
        // Deci problema se reduce la verificarea faptului ca exista o secventa de elemente din A (dar nu intreaga multime A),
        //  a carei medie e egala cu media lui A

        // Ideea de baza -> sortam vectorul si ne apropiem putin cate putin de medie(A) plecand de la un subset SS continand cele mai apropiate
        //   doua elemente de medie(A); adaugam la acest set valori noi in functie de comparatia intre medie(A) si medie(SS)
        public static bool CanBeSplitIntoTwoEqualAverages(List<double> nums)
        {
            // daca avem 0 sau 1 element, nu se poate
            if (nums.Count < 2)
            {
                return false;
            }

            // daca A contine media ca si element -> OK
            //  conform calculelor de mai sus, si media restului elementelor este egala cu media lui A
            double sum = nums.Sum();
            double average = nums.Average();
            if (nums.Contains(average))
            {
                return true;
            }

            // Sortam vectorul initial
            nums.Sort();

            // Incat nu avem un element egal cu media, trebuie sa gasim o submultime (elemente consecutive) cu cel putin doua elemente
            //  Pentru inceput luam cele mai apropiate doua elemente de medie
            //   si anume cel mai mare numar mai mic decat media + cel de la pozitia urmatoare (am facut sortarea, deci acest lucru este garantat)
            int lower = nums.FindLastIndex(x => x < average);
            int upper = lower + 1;
            // Media de start
            double iterAvg = (nums[lower] + nums[upper]) / 2;
            while (lower >= 0 && upper < nums.Count)
            {
                // Am ajuns la capetele vectorului, ceea ce inseamna ca unica submultime cu media egala cu medie(A) este chiar A
                //      => Fals, pentru ca C ramane gol
                if (lower == 0 && upper == nums.Count - 1)
                {
                    return false;
                }

                // Media secventei continue este egala cu medie(A) si C nu ramane gol -> OK
                if (iterAvg == average)
                {
                    return true;
                }
                else
                {
                    // Avem b - a + 1 valori in intervalul [a, b]
                    int countTillNow = upper - lower + 1;
                    bool foundNew = false;
                    // Daca media subsecventei este mai mica decat medie(A), atunci trebuie sa alegem un element mai mare decat medie(A)
                    // pentru a ne apropia de aceasta medie -> luam urmatorul element mai mare decat medie(A), daca exista
                    if (iterAvg < average)
                    {
                        if (upper < nums.Count - 1)
                        {
                            upper++;
                            foundNew = true;
                        }
                    }
                    // In mod similar, daca media subsecventei e mai mare decat medie(A), atunci unica metoda de a ne apropia de aceasta medie
                    // este sa luam un element mai mic decat ea
                    else
                    {
                        if (lower > 0)
                        {
                            lower--;
                            foundNew = true;
                        }
                    }

                    // calculam noua medie a intervalului [low, high], daca cele doua valori nu depasesc limitele
                    if (foundNew)
                    {
                        iterAvg = nums.GetRange(lower, countTillNow + 1).Average();
                    }
                }
            }

            // daca ajungem aici, capetele vectorului au fost depasite si nu am gasit nicio subsecventa cu proprietarea ceruta -> Fals
            return false;
        }
    }
}
