using System.Runtime.CompilerServices;

namespace Graphssss
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph();
            Console.WriteLine("Введите количество узлов");
            int edge_count = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Граф взвешенный? +-");
            string vzvesh = Console.ReadLine();
            if (vzvesh == "+")
            {
                Console.WriteLine("Введите список смежности построчно в формате\r\n" +
                "<название вершины> <название вершины> <длина ребра>....");
            }
            else if (vzvesh == "-")
            {
                Console.WriteLine("Введите список смежности построчно в формате\r\n" +
                "<название вершины> <название вершины>....");
            }    
            for (int i = 0; i < edge_count; i++)
            {
                string s = Console.ReadLine();
                string[] str = s.Split(' ');
                string name_from = str[0];
                if (vzvesh == "+")
                {
                    for (int j = 1; j < str.Length - 1; j += 2)
                    {
                        string name_to = str[j];
                        int length = int.Parse(str[j + 1]);
                        graph.add_e(name_from, name_to, length);
                    }
                }
                else if (vzvesh == "-")
                {
                    for (int j = 1; j < str.Length; j++)
                    {
                        string name_to = str[j];
                        graph.add_e(name_from, name_to, 1);
                    }
                }
            }
            graph.output();
            graph.AllPaths(14);
        }
    }
    public class Vertex
    {
        public string name;
        public string mark;

        public Vertex(string name, string mark = "white")
        {
            this.name = name;
            this.mark = mark;
        }
    }
    
    public class Edge
    {
        public Vertex vertex;
        public int length;

        public Edge(Vertex vertex, int length)
        {
            this.vertex = vertex;
            this.length = length;
        }
    }

    public class Graph
    {
        public List<Vertex> vertexes = new List<Vertex>();
        public List<(string vertex_name, List<Edge> edges)> adj_dict = new List<(string vertex_name, List<Edge> edges)>();
        public int edges_count = 0;
        public int vertexes_count = 0;
        public bool directed;

        public Graph(bool directed = true)
        {
            this.directed = directed;
        }

        public void add_v(string name)
        {
            bool flag = false;
            foreach (var vertex in vertexes)
            {
                if (vertex.name == name)
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                vertexes.Add(new Vertex(name));
                adj_dict.Add(new (name, new List<Edge>()));
                vertexes_count++;
            }
        }

        public void del_v(string name)
        {
            foreach (var vertex in vertexes)
            {
                if (vertex.name == name)
                {
                    vertexes.Remove(vertex);
                }
            }

            foreach (var vertex_cort in adj_dict)
            {
                if (vertex_cort.Item1 == name)
                {
                    adj_dict.Remove(vertex_cort);
                }
                foreach (var edge in vertex_cort.Item2)
                {
                    if (edge.vertex.name == name)
                    {
                        vertex_cort.Item2.Remove(edge);
                        break;
                    }
                }
            }
            vertexes_count--;
        }

        public string first(string name) //Вернуть название первой вершины, связанной с name
        {
            foreach(var vertex_cort in adj_dict)
            {
                if (vertex_cort.Item1 == name)
                {
                    return vertex_cort.Item2[0].vertex.name;
                }
            }
            return "something went wrong ^_^";
        }

        public string next(string name, int index) // Вернуть название следующей после index вершины, связанной с name, либо индекс name
        {
            foreach (var vertex_cort in adj_dict)
            {
                if (vertex_cort.Item1 == name)
                {
                    return vertex_cort.Item2[index + 1].vertex.name;
                }
            }
            return "something went wrong ^_^";
        }

        public string vertex(string name, int index) // Вернуть название вершины с индексом i из множества вершин, связанных с v
        {
            foreach (var vertex_cort in adj_dict)
            {
                if (vertex_cort.Item1 == name)
                {
                    return vertex_cort.Item2[index].vertex.name;
                }
            }
            return "something went wrong ^_^";
        }

        public void add_e(string name_from, string name_to, int length)// Добавить ребро между вершинами v и w с длиной c
        {
            Vertex ver_from = null;
            Vertex ver_to = null;
            foreach (var vertex in vertexes)
            {
                if (vertex.name == name_from)
                {
                    ver_from = vertex;
                }
                if (vertex.name == name_to)
                {
                    ver_to = vertex;
                }
            }
            if (ver_from == null)
            {
                add_v(name_from);
                foreach (var vertex in vertexes)
                {
                    if (vertex.name == name_from)
                    {
                        ver_from = vertex;
                    }
                }
            }
            if (ver_to == null)
            {
                add_v(name_to);
                foreach (var vertex in vertexes)
                {
                    if (vertex.name == name_to)
                    {
                        ver_to = vertex;
                    }
                }
            }
            foreach (var vertex_cort in adj_dict)
            {
                if (vertex_cort.Item1 == name_from)
                {
                    vertex_cort.Item2.Add(new Edge(ver_to, length));
                }
                if (vertex_cort.Item1 == name_to && !directed)
                {
                    vertex_cort.Item2.Add(new Edge(ver_from, length));
                }
            }
            edges_count++;
        }

        public void del_e(string name_from, string name_to) //Удалить ребро между v и w
        {
            foreach (var vertex_cort in adj_dict)
            {
                if (vertex_cort.Item1 == name_from)
                {
                    foreach (var edge in vertex_cort.Item2)
                    {
                        if (edge.vertex.name == name_to)
                        {
                            vertex_cort.Item2.Remove(edge);
                        }
                    }
                }
            }
            if (!directed)
            {
                foreach (var vertex_cort in adj_dict)
                {
                    if (vertex_cort.Item1 == name_to)
                    {
                        foreach (var edge in vertex_cort.Item2)
                        {
                            if (edge.vertex.name == name_from)
                            {
                                vertex_cort.Item2.Remove(edge);
                            }
                        }
                    }
                }
            }
            edges_count--;
        }

        public void edit_v(string v, string mark) // Изменить маркировку вершины
        {
            foreach(var vertex in vertexes)
            {
                if (vertex.name == v)
                {
                    vertex.name = mark;
                }
            }
        }

        public void edit_e(string name_from, string name_to, int length) // Изменить вес ребра между name_from и name_to на length
        {
            foreach (var vertex_cort in adj_dict)
            {
                if (vertex_cort.Item1 == name_from)
                {
                    foreach (var edge in vertex_cort.Item2)
                    {
                        if (edge.vertex.name == name_to)
                        {
                            edge.length = length;
                        }
                    }
                }
            }
            if (!directed)
            {
                foreach (var vertex_cort in adj_dict)
                {
                    if (vertex_cort.Item1 == name_to)
                    {
                        foreach (var edge in vertex_cort.Item2)
                        {
                            if (edge.vertex.name == name_from)
                            {
                                edge.length = length;
                            }
                        }
                    }
                }
            }
        }

        public void output()
        {
            string s = "Graph\r\n";
            foreach (var vertex_cort in adj_dict)
            {
                s += $"From: {vertex_cort.Item1}:";
                foreach (var edge in vertex_cort.Item2)
                {
                    s += $"  {edge.vertex.name}";
                }
                s += "\r\n";
            }
            Console.WriteLine(s);
        }

        public int count_components()
        {
            int count = 0;
            foreach (var vertex in vertexes)
            {
                count++;
            }
            return count;
        }

        public void AllPaths(int goal_length)
        {
            List<List<string>> ways = new List<List<string>>();
            foreach (var vertex in vertexes)
            {
                List<string> way = new List<string>();
                Iter(vertex, ways, goal_length, way);

            }
            foreach (var way in ways)
            {
                Console.Write("путь:");
                foreach (var vertex in way)
                {
                    Console.Write($" {vertex} ");
                }
                Console.WriteLine();
            }
        }
        private void Iter(Vertex vertex, List<List<string>> ways, int goal_lenght, List<string> way)
        {
            way.Add(vertex.name);
            if (goal_lenght == 0) ways.Add(way);
            int start_len_count = goal_lenght;
            foreach (var vertex_cort in adj_dict)
            {
                if (vertex_cort.Item1 == vertex.name)
                {
                    
                    foreach (var edge in vertex_cort.Item2)
                    {
                        if (!Find(edge.vertex.name, way) && goal_lenght - edge.length >= 0)
                        {
                            List<string> new_way = new List<string>();
                            new_way.AddRange(way);
                            
                            goal_lenght -= edge.length;
                            Iter(edge.vertex, ways, goal_lenght, new_way);
                            
                        }
                        if (start_len_count > goal_lenght)
                        {
                            goal_lenght = start_len_count;
                        }
                    }
                    break;
                }
            }
        }

        private bool Find(string vertex_name, List<string> way)
        {
            foreach (var vertex in way)
            {
                if (vertex_name == vertex)
                {
                    return true;
                }
            }
            return false;
        }
    }
}