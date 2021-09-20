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
            Console.WriteLine("Введите количество вершин:");
            string n = Console.ReadLine();
        
            Console.WriteLine("Введите количество рёбер:");
            string m = Console.ReadLine();
            int i = 0;
            Console.WriteLine("Вводите смежные вершины, сначала вершину из которой выходит ребро, затем вторую:");
            while (i <= int.Parse(n) | i <= int.Parse(m)) 
            {
                Console.Write("Первая вершина: ");
                string v1 = Console.ReadLine();
                Console.Write("Первая вершина: ");
                string v2 = Console.ReadLine();
                myEdges.Add(new Edge(int.Parse(v1), int.Parse(v2)));
                i++; // Увеличиваем счетчик.
                Console.Write("Введите следующую пару вершин: \n");
            }

            //Пусть имеется такой граф(список дуг):

            List<Subgraph> subgraphs = new List<Subgraph>();

            MyAlgorithm.TopologicalDecomposition(10, myEdges, subgraphs);
            
            for (i = 0; i < subgraphs.Count; i++) {
                Subgraph subgraph = subgraphs[i];
              
                string resString = $"Подграф_{i + 1}: (";
                for (int j = 0; j < subgraph.Peaks.Count; j++)
                {
                    int peak = subgraph.Peaks[j];
                    resString += $"{peak}";
                    if (j < (subgraph.Peaks.Count - 1)) {
                        resString += ", ";
                    };
                        
                };
                resString += ")";

                Console.WriteLine(resString);
            };
           
            Console.WriteLine("END");
            Console.ReadKey();
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
        /// выполняет топологическую декомпозицию структуры
        /// </summary>
        /// <param name="countV">количество вершин в графе</param>
        /// <param name="E">список дуг в графе</param>
        /// <param name="Sub">список подграфов</param>
        public static void TopologicalDecomposition(int countV, List<Edge> E, List<Subgraph> Sub)
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
    }
}
