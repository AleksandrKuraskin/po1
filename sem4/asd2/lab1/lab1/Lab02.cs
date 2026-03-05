using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASD
{
    public class Lab02 : MarshalByRefObject
    {
        /// <summary>
        /// Optymalne rozmieszczenie parasolek w wariancie, w którym każda parasolka ma taki sam promień
        /// oraz mamy do dyspozycji tylko zadaną liczbę parasolek (rozmieszczenie parasolek nie wiąże się z żadnym kosztem)
        /// </summary>
        /// <param name="Z">Tablica zysków, Z[i] to zysk za pokrycie punktu o numerze i</param>
        /// <param name="umbrellaCount">Liczba dostępnych parasolek</param>
        /// <param name="umbrellaRadius">Promień parasolki (parasolka o promieniu r umieszczona w punkcie i pokrywa punkty i-r, i-r+1, ..., i+r)</param>
        /// <returns></returns>
        public (int profit, int[] umbrellaPosition) Stage1(int[] Z, int umbrellaCount, int umbrellaRadius)
        {
            var cL = Z.Length;
            var Zs = new int[cL + 1];
            var d = 2 * umbrellaRadius + 1;
            Zs[0] = 0;
            for (var i = 1; i <= cL; i++)
            {
                Zs[i] = Z[i-1] + Zs[i - 1];
            }
            var T = new int[umbrellaCount,cL];

            for (var u = 0; u < umbrellaCount; u++)
            {
                for (var c = 0; c < cL; c++)
                {
                    if (c < d)
                    {
                        T[u, c] = Zs[c + 1];
                        continue;
                    }
                    if (u == 0)
                    {
                        var inRad = Zs[c + 1] - Zs[c + 1 - d];
                        if (inRad > T[u, c - 1])
                        {
                            T[u, c] = inRad;
                        }
                        else
                        {
                            T[u, c] = T[u, c - 1];
                        }
                    }
                    else
                    {
                        T[u, c] = T[u - 1, c - d] + (Zs[c + 1] - Zs[c + 1 - d]);
                    }
                }
            }

            var profit = T[umbrellaCount - 1, cL - 1];
            
            return (profit, null);
        }


        /// <summary>
        /// Optymalne rozmieszczenie parasolek w wariancie, w którym mamy dostępne modele parasolek o różnych promieniach.
        /// Każdego modelu możemy użyć dowolną liczbę razy, jednak za każdym razem musimy ponieść jego koszt.
        /// </summary>
        /// <param name="Z">Tablica zysków, Z[i] to zysk za pokrycie punktu o numerze i</param>
        /// <param name="umbrellaType">Tablice dostępnych modeli parasolek, gdzie i-ty model ma promień umbrellaType[i].radius i koszt umbrellaType[i].cost</param>
        /// <returns></returns>
        public (int profit, (int position, int model)[] umbrellas) Stage2(int[] Z, (int radius, int cost)[] umbrellaType)
        {
            return (0, null);
        }
    }
}
