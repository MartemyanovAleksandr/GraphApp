using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphApp
{
    class Program
    {
        static void Main(string[] args)
        {

            List<Edge> myEdges = new List<Edge>();
            List<Subgraph> mySubgraphs = new List<Subgraph>();

            //Пусть имеется такой граф(список дуг):           
            //myEdges.Add(new Edge(1, 2));
            //myEdges.Add(new Edge(3, 2));
            //myEdges.Add(new Edge(4, 3));
            //myEdges.Add(new Edge(4, 5));
            //myEdges.Add(new Edge(6, 5));
            //myEdges.Add(new Edge(7, 6));
            //myEdges.Add(new Edge(7, 8));
            //myEdges.Add(new Edge(8, 9));
            //myEdges.Add(new Edge(9, 1));
            //myEdges.Add(new Edge(2, 7));
            //myEdges.Add(new Edge(3, 10));
            //myEdges.Add(new Edge(4, 8));
            //myEdges.Add(new Edge(6, 10));
            //myEdges.Add(new Edge(10, 9));

            //начинать надо с 0, а на с 1 - тупой алгоритм
            myEdges.Add(new Edge(0, 1));
            myEdges.Add(new Edge(2, 1));
            myEdges.Add(new Edge(3, 2));
            myEdges.Add(new Edge(3, 4));
            myEdges.Add(new Edge(5, 4));
            myEdges.Add(new Edge(6, 5));
            myEdges.Add(new Edge(6, 7));
            myEdges.Add(new Edge(7, 8));
            myEdges.Add(new Edge(8, 0));
            myEdges.Add(new Edge(1, 6));
            myEdges.Add(new Edge(2, 9));
            myEdges.Add(new Edge(3, 7));
            myEdges.Add(new Edge(5, 9));
            myEdges.Add(new Edge(9, 8));

            #region enterEdges
            //Console.WriteLine("Введите количество дуг:");
            //int countEdges = int.Parse(Console.ReadLine());

            //Console.WriteLine("Чтобы задать дугу введите начальную и конечную вершины через запятую, например 5,7.");
            //for (int i = 0; i < countEdges; i++)
            //{
            //    Console.WriteLine($"Задайте дугу номер {i + 1}.");
            //    string[] v = Console.ReadLine().Trim().Split(",");

            //    int beginPeak = int.Parse(v[0]) - 1;
            //    int endPeak = int.Parse(v[1]) - 1;

            //    if (beginPeak < 0 || endPeak < 0)
            //    {
            //        Console.WriteLine($"Вершины не могут быть меньше 1.");
            //        i--;
            //        continue;
            //    }
            //    else
            //        myEdges.Add(new Edge(beginPeak, endPeak));
            //};
            #endregion enterEdges

            List<int> peaks = new List<int>();
            myEdges.ForEach(edge =>
            {
                peaks.Add(edge.beginPoint);
                peaks.Add(edge.endPoint);
            });
            int countV = peaks.Distinct().Count();

            MyAlgorithm.TopologicalDecomposition(countV, myEdges, mySubgraphs);
            MyAlgorithm.PrintAltRes(myEdges, mySubgraphs);
            //MyAlgorithm.PrintRes(mySubgraphs);

        }
    }

    public class Edge
    {
        public int beginPoint, endPoint;
     
        /// <param name="_beginPoint">номер вершины, из которой дуга исходит</param>
        /// <param name="_endPoint">номер вершины, в которую данная дуга заходит</param>
        public Edge(int _beginPoint, int _endPoint)
        {
            this.beginPoint = _beginPoint;
            this.endPoint = _endPoint;
        }
    }

    public class Subgraph
    {
        public List<int> Peaks;
        public string Name;
       
        public Subgraph()
        {
            Peaks = new List<int>();
        }
        public Subgraph(List<int> V)
        {
            this.Peaks = new List<int>(V);
        }

    }

    public class MyAlgorithm {

        /// <summary>
        /// Если вершина endVIndex достижима из вершины beginVIndex, то метод вернет значение true, иначе false.
        /// </summary>
        /// <param name="beginVIndex">номер начальной вершины</param>
        /// <param name="endVIndex">номер конечной вершины</param>
        /// <param name="Edges"> список дуг графа</param>
        /// <param name="colors">цвета вершин</param>       
        static bool DFS(int beginVIndex, int endVIndex, List<Edge> Edges, int[] colors)
        {
            colors[beginVIndex] = 2;
            if (beginVIndex == endVIndex)
            {
                return true;
            }
            for (int w = 0; w < Edges.Count; w++)
            {
                Edge itemEdge = Edges[w];
                int colorIndex = itemEdge.endPoint;
                if (colors[colorIndex] == 1 && itemEdge.beginPoint == beginVIndex)
                {
                    if (DFS(itemEdge.endPoint, endVIndex, Edges, colors)) return true;
                    colors[colorIndex] = 1;
                }
            }
            return false;
        }

        /// <summary>
        /// выполняет топологическую декомпозицию структуры, вершины должны начинаться с 0
        /// </summary>
        /// <param name="countV">количество вершин в графе</param>
        /// <param name="E">список дуг в графе</param>
        /// <param name="Sub">список подграфов</param>
        static public void TopologicalDecomposition(int countV, List<Edge> E, List<Subgraph> Sub)
        {
            List<int> notUsedV = new List<int>(); //список еще не использованных вершин
            for (int i = 0; i < countV; i++)
                notUsedV.Add(i);
            while (notUsedV.Count > 0)
            {
                List<int> R = new List<int>(); //достижимое множество
                R.Add(notUsedV[0]);
                List<int> Q = new List<int>(); //контрдостижимое множество
                Q.Add(notUsedV[0]);
                int[] colors = new int[countV];
                //формируем достижимое и контрдостижимое множества
                for (int i = 1; i < notUsedV.Count; i++)
                {
                    for (int k = 0; k < countV; k++)
                    {
                        if (notUsedV.IndexOf(k) != -1)
                            colors[k] = 1;
                        else
                            colors[k] = 2;
                    }
                    if (DFS(notUsedV[0], notUsedV[i], E, colors)) R.Add(notUsedV[i]);
                    for (int k = 0; k < countV; k++)
                    {
                        if (notUsedV.IndexOf(k) != -1)
                            colors[k] = 1;
                        else
                            colors[k] = 2;
                    }
                    if (DFS(notUsedV[i], notUsedV[0], E, colors)) Q.Add(notUsedV[i]);
                }
                //пересечение множеств R и Q
                List<int> intersection = new List<int>(R.Intersect(Q).ToList());
                Sub.Add(new Subgraph(intersection));
                for (int i = 0; i < intersection.Count; i++)
                {
                    notUsedV.Remove(intersection[i]);
                }
            }
        }
        
        static public void PrintRes(List<Subgraph> subgraphs) {
            Console.WriteLine("BEGIN");
            for (int i = 0; i < subgraphs.Count; i++)
            {
                Subgraph subgraph = subgraphs[i];

                string resString = $"Подграф_{i + 1}: (";
                for (int j = 0; j < subgraph.Peaks.Count; j++)
                {
                    int peak = subgraph.Peaks[j];
                    resString += $"{peak + 1}";
                    if (j < (subgraph.Peaks.Count - 1))
                    {
                        resString += ", ";
                    };

                };
                resString += ")";

                Console.WriteLine(resString);
            };
            Console.WriteLine("END");
            Console.ReadKey();
        }

        static public void PrintAltRes(List<Edge> edges, List<Subgraph> subgraphs) 
        {
            IEnumerable<int> peaks = subgraphs.SelectMany(x => x.Peaks);
            IEnumerable<int> inputPeaks = edges.Select(x => x.endPoint).Where(x => peaks.Contains(x)).Distinct();
            IEnumerable<int> outputPeaks = edges.Select(x => x.beginPoint).Where(x => peaks.Contains(x)).Distinct();

            //именуем подграфы
            for (int i = 0; i < subgraphs.Count(); i++) {
                subgraphs[i].Name = $"G_{i+1}";
            };

            subgraphs.ForEach(subgraph => {
                //вершины, через котрые текущий граф "входит" в другие графы
                List<int> siblingInputPeaks = new List<int>();
               
                //по всем исходящм вершинам текущего графа
                outputPeaks.Where(peak => subgraph.Peaks.Contains(peak)).ToList().ForEach(subgraphOutputPeak => {
                    //по всем входящим вершинам исключая вершины текущего графа
                    inputPeaks.Where(inputPeak => !subgraph.Peaks.Contains(inputPeak)).ToList().ForEach(siblingInputPeak => {
                        //если есть дуга, которая соединяет текущую вершину с другой вершиной, не принадлежащей текущему графу 
                        Edge linkEdge = edges.FirstOrDefault(e => e.beginPoint == subgraphOutputPeak && e.endPoint == siblingInputPeak);
                        if (linkEdge != null && !siblingInputPeaks.Contains(siblingInputPeak)) {
                            siblingInputPeaks.Add(siblingInputPeak);
                        };
                    });
                });

                //смотрим каким графам принадлежит список "соседских входных вершин"
                IEnumerable<Subgraph> siblingSubgraphs = subgraphs
                    .Where(sg => (sg.Name != subgraph.Name) && (siblingInputPeaks.Distinct().Intersect(sg.Peaks).Count() > 0));

                string res = $"G({subgraph.Name} => ";
                if (siblingSubgraphs != null && siblingSubgraphs.Count() > 0)
                    res += $"{string.Join(",", siblingSubgraphs.Select(s => s.Name))}";
                else
                    res += "0";

                res += ")";
                Console.WriteLine(res);
            });
            Console.ReadKey();
        }
    }        
}
