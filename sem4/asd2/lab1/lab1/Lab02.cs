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
            for (var i = 0; i < cL; i++)
            {
                Zs[i+1] = Z[i] + Zs[i];
            }
            var T = new int[umbrellaCount + 1, cL + 1];
            var U = new int[umbrellaCount + 1, cL + 1];

            for (var u = 1; u <= umbrellaCount; u++)
            {
                for (var c = 1; c <= cL; c++)
                {
                    var ind = Math.Max(0, c - d);
                    T[u, c] = T[u, c - 1];
                    
                    var cP = T[u - 1, ind] + (Zs[c] - Zs[ind]);
                    if (cP > T[u, c])
                    {
                        T[u, c] = cP;
                        U[u, c] = ind;
                    }
                }
            }

            var uP = new int[umbrellaCount];
            var uC = umbrellaCount;
            var cI = cL;

            while (uC > 0 && cI > 0)
            {
                var p = U[uC, cI];
                if (p != 0)
                {
                    uP[uC - 1] = cI - 1 - umbrellaRadius;
                    cI = p;
                    uC--;
                }
                else
                {
                    cI--;
                }
            }
            
            var profit = T[umbrellaCount, cL];
            return (profit, uP);
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
