using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Proyecto_U5_y_U6
{

    class Comentario 
    {
        public int Id { get; set; }
        public string Autor { get; set; }
        public DateTime FechaDePublicacion { get; set; }
        public string comentario { get; set; }
        public string Ip { get; set; }
        public int Inapropiado { get; set; }
        public int Likes { get; set; }

        public override string ToString()
        {
            return String.Format($"Id = {Id} - Autor: {Autor} - Fecha: {FechaDePublicacion} - {comentario} - {Ip} - Likes: {Likes} - {Inapropiado} personas lo han marcado como inapropiado");
        }
    }

    class ComentarioDB
    {
        public static void SaveToFile(List<Comentario> comentarios, string path)
        {
            StreamWriter textOut = null;

            string[] words = { "puto", "culo", "chingar", "chingaste", "mierda", "verga", "mamon" };

            try
            {
                textOut = new StreamWriter(new FileStream(path, FileMode.Create, FileAccess.Write));
                foreach (var comentario in comentarios)
                {
                    textOut.Write(comentario.Id + " | ");
                    textOut.Write(comentario.Autor + " | ");
                    textOut.Write(comentario.FechaDePublicacion + " | ");
                    // Filtro para malas palabras
                    if (words.Any(comentario.comentario.Contains))
                    {
                        if(comentario.Inapropiado > 0)    
                        {
                            textOut.Write("************" + " | ");
                        }
                        else 
                        {
                            textOut.Write(comentario.comentario + " | "); 
                        }
                    }
                    else
                    {
                        textOut.Write(comentario.comentario + " | ");
                    }
                    textOut.Write(comentario.Ip + " | ");
                    textOut.Write(comentario.Likes + " | ");
                    textOut.WriteLine(comentario.Inapropiado);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
            }

            catch (Exception)
            {
                Console.WriteLine("Error");
            }
            finally
            {
                if (textOut != null)
                    textOut.Close();
            }
        }

        public static List<Comentario> ReadFromFile(string path)
        {
            List<Comentario> comentarios = new List<Comentario>();

            StreamReader textIn = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read));

            try
            {
                while (textIn.Peek() != -1)
                {
                    string row = textIn.ReadLine();
                    string[] columns = row.Split('|');
                    Comentario c = new Comentario();
                    c.Id = int.Parse(columns[0]);
                    c.Autor = columns[1];
                    c.FechaDePublicacion = DateTime.Parse(columns[2]);
                    c.comentario = columns[3];
                    c.Ip = columns[4];
                    c.Likes = int.Parse(columns[5]);
                    c.Inapropiado = int.Parse(columns[6]);
                    comentarios.Add(c);
                }
            }
            catch (IOException)
            {
                Console.WriteLine("No existe el archivo");
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
            }
            textIn.Close();
            return comentarios;
        }

        public static void OrdenaxLikes(string path)
        {
            List<Comentario> comentarios;
         
            comentarios = ReadFromFile(path);

            var ordena_comentario = from c in comentarios orderby c.Likes descending select c;

            foreach (var c in ordena_comentario)
            {
                Console.WriteLine(c);
            }
        }

        public static void OrdenaxtIiempo(string path)
        {
            List<Comentario> comentarios;

            comentarios = ReadFromFile(path);

            var ordena_comentario = from c in comentarios orderby c.FechaDePublicacion descending
                                    select c;

            foreach (var c in ordena_comentario)
            {
                Console.WriteLine(c);
            }
        }

        public static void Imprimir(string path)
        {
            List<Comentario> comentarios;

            comentarios = ReadFromFile(path);

            foreach (var c in comentarios)
            {
                Console.WriteLine(c);
            }
        }
    }

    delegate bool FindComentario(Comentario c);

    class ComentarioExtension
    {
        public static List<Comentario> Where(List<Comentario> comentarios, FindComentario del)
        {
            List<Comentario> filtro_comentario = new List<Comentario>();

            foreach (var c in comentarios)
            {
                if (del(c))
                {
                    filtro_comentario.Add(c);
                }
            }
            return filtro_comentario;
        }
    }
      
    class Program
    {
        static void Main(string[] args)
        {
            List<Comentario> comentarios = new List<Comentario>();
            
            comentarios.Add(new Comentario() { Id = 001, Autor = "Antonio_98", FechaDePublicacion = DateTime.Now, comentario = "mamon", Ip = "192.301.912", Likes = 18, Inapropiado = 2 });
            comentarios.Add(new Comentario() { Id = 002, Autor = "Luis_099", FechaDePublicacion = DateTime.Now, comentario = "Hola que tal", Ip = "192.301.912", Likes = 99, Inapropiado = 0 });
            comentarios.Add(new Comentario() { Id = 003, Autor = "Montes123", FechaDePublicacion = DateTime.Now, comentario = "Eres culo", Ip = "192.301.912", Likes = 27, Inapropiado = 8 });
            comentarios.Add(new Comentario() { Id = 004, Autor = "Ojeda_Ojeda", FechaDePublicacion = DateTime.Now, comentario = "Comer mierda", Ip = "192.301.912", Likes = 4, Inapropiado = 12});

            ComentarioDB.SaveToFile(comentarios, @"C:\Users\Antonio\Comentario.txt");           
            
            // Recuperar del archivo de texto y mostrarlos 
            ComentarioDB.ReadFromFile(@"C:\Users\Antonio\Comentario.txt");

            ComentarioDB.Imprimir(@"C:\Users\Antonio\Comentario.txt");
            Console.WriteLine();

            for (var i = 0; i != 4; i = i++)
            {
                try
                {
                    Console.WriteLine("Escriba 1: Borrar por id, 2: Ordenar por Likes, 3: Ordenar por fecha, 4: Salir");

                    int seleccionador = Convert.ToInt32(Console.ReadLine());
                    i = seleccionador;

                    if (seleccionador > 0 && seleccionador < 5)
                    {
                        if (seleccionador == 1)
                        {
                            // borra por id
                            Console.WriteLine("Ingrese la Id del comentario");
                            int Borrar_Id;
                            Borrar_Id = Convert.ToInt32(Console.ReadLine());
                            comentarios.RemoveAll(b => b.Id == Borrar_Id);
                            foreach (var c in comentarios)
                            {
                                Console.WriteLine(c);
                            }
                            ComentarioDB.SaveToFile(comentarios, @"C:\Users\Antonio\Comentario.txt");
                        }
                        else if (seleccionador == 2)
                        {
                            // filtro por likes
                            ComentarioDB.OrdenaxLikes(@"C:\Users\Antonio\Comentario.txt");
                        }
                        else if (seleccionador == 3)
                        {
                            // Filtro por tiempo
                            ComentarioDB.OrdenaxtIiempo(@"C:\Users\Antonio\Comentario.txt");
                        }

                        Console.WriteLine("Presione cualquier tecla para continuar");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else
                    {
                        Console.WriteLine("Esa no es una opcion valida");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Ingreso un dato incorrecto");
                }
                catch (Exception)
                {
                    Console.WriteLine("No ingreso ninguna opcion");
                }
            }
            Console.WriteLine("Presione cualquier tecla para finalizar");
            Console.ReadKey();
        }
    }
}

