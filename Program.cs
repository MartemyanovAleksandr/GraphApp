using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //Пусть имеется такой граф(список дуг):
            List<Edge> myEdges = new List<Edge>();
            //myEdges.Add(new Edge(1,2));
            //myEdges.Add(new Edge(3,2));
            //myEdges.Add(new Edge(4,3));
            //myEdges.Add(new Edge(4,5));
            //myEdges.Add(new Edge(6,5));
            //myEdges.Add(new Edge(7,6));
            //myEdges.Add(new Edge(7,8));
            //myEdges.Add(new Edge(8,9));
            //myEdges.Add(new Edge(9,1));
            //myEdges.Add(new Edge(2,7));
            //myEdges.Add(new Edge(3,10));
            //myEdges.Add(new Edge(4,8));
            //myEdges.Add(new Edge(6,10));
            //myEdges.Add(new Edge(10,9));

            //начинать надо с нуля, а на с 1 - тупой алгоритм
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

            List<Subgraph> subgraphs = new List<Subgraph>();

            MyAlgorithm.TopologicalDecomposition(10, myEdges, subgraphs);

            Console.WriteLine("END");
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
        public List<int> V;

        public Subgraph()
        {
            V = new List<int>();
        }
        public Subgraph(List<int> V)
        {
            this.V = new List<int>(V);
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
