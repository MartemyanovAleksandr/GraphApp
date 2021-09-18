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
            List<Edge> MyEdges = new List<Edge>();
            MyEdges.Add(new Edge(1,2));
            MyEdges.Add(new Edge(3,2));
            MyEdges.Add(new Edge(4,3));
            MyEdges.Add(new Edge(4,5));
            MyEdges.Add(new Edge(6,5));
            MyEdges.Add(new Edge(7,6));
            MyEdges.Add(new Edge(7,8));
            MyEdges.Add(new Edge(8,9));
            MyEdges.Add(new Edge(9,1));
            MyEdges.Add(new Edge(2,7));
            MyEdges.Add(new Edge(3,10));
            MyEdges.Add(new Edge(4,8));
            MyEdges.Add(new Edge(6,10));
            MyEdges.Add(new Edge(10,9));

            List<Subgraph> res = new List<Subgraph>();

            MyAlgorithm.TopologicalDecomposition(10, MyEdges, res);

            Console.WriteLine("END");
        }
    }

    public class Edge
    {
        public int v1, v2;
     
        /// <param name="v1">номер вершины, из которой дуга исходит</param>
        /// <param name="v2">номер вершины, в которую данная дуга заходит</param>
        public Edge(int v1, int v2)
        {
            this.v1 = v1;
            this.v2 = v2;
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
        /// 
        /// </summary>
        /// <param name="u">номер начальной вершины</param>
        /// <param name="endV">номер конечной вершины</param>
        /// <param name="E"> список дуг графа</param>
        /// <param name="color">цвета вершин</param>
        /// <returns>Если вершина endV достижима из вершины u, то метод вернет значение true, иначе false.</returns>
        static bool DFS(int u, int endV, List<Edge> E, int[] color)
        {
            color[u] = 2;
            if (u == endV)
            {
                return true;
            }
            for (int w = 0; w < E.Count; w++)
            {
                if (color[E[w].v2] == 1 && E[w].v1 == u)
                {
                    if (DFS(E[w].v2, endV, E, color)) return true;
                    color[E[w].v2] = 1;
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
                int[] color = new int[countV];
                //формируем достижимое и контрдостижимое множества
                for (int i = 1; i < notUsedV.Count; i++)
                {
                    for (int k = 0; k < countV; k++)
                    {
                        if (notUsedV.IndexOf(k) != -1)
                            color[k] = 1;
                        else
                            color[k] = 2;
                    }
                    if (DFS(notUsedV[0], notUsedV[i], E, color)) R.Add(notUsedV[i]);
                    for (int k = 0; k < countV; k++)
                    {
                        if (notUsedV.IndexOf(k) != -1)
                            color[k] = 1;
                        else
                            color[k] = 2;
                    }
                    if (DFS(notUsedV[i], notUsedV[0], E, color)) Q.Add(notUsedV[i]);
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
