/*
 * User: m4rc3lo
 * Date: 12/10/2019
 * Time: 00:42 
 */
using System;

namespace TTT
{
	//representacao e desenho do tabuleiro
	public class Desenho
	{
		//data
		private static DateTime data;
		//construtor vazio
		public Desenho()
		{
			data = DateTime.Now;
		}
		// mostra tela inicial
		public static void apresentacao ()
		{
			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.WriteLine("--------------------------------------------------");
			Console.WriteLine("Jogo: Tic Tac Toy");
			Console.WriteLine(data);
			Console.WriteLine("--------------------------------------------------");
			Console.ResetColor();
		}
		
		// mostra pessoa a iniciar
		public static void mostra_first(Pessoa j1, Pessoa j2)
		{
			if (j1.get_first())
			{
				Console.WriteLine(" ");
				Console.WriteLine("Primeira pessoa a jogar: " + j1.get_nome());
				Console.WriteLine("Simbolo: " + j1.get_simbolo());
			}
			else
			{
				Console.WriteLine(" ");
				Console.WriteLine("Primeira pessoa a jogar: " + j2.get_nome());
				Console.WriteLine("Simbolo: " + j2.get_simbolo());
			}
			Console.WriteLine("Precione uma tecla para ler as instrucoes");
			Console.ReadKey(true);
			Console.Clear();
		}
		
		// mostra tabuleiro no terminal
		public static void desenha_tabuleiro(char[,] tabuleiro)
		{
			Console.WriteLine("----------------------------------------------------");
			Console.WriteLine("     |     |      ");
			Console.WriteLine("  {0}  |  {1}  |  {2}", tabuleiro[0,0], tabuleiro[0,1], tabuleiro[0,2]);
			Console.WriteLine("_____|_____|_____ ");
			Console.WriteLine("     |     |      ");
			Console.WriteLine("  {0}  |  {1}  |  in{2}", tabuleiro[1,0], tabuleiro[1,1], tabuleiro[1,2]);
			Console.WriteLine("_____|_____|_____ ");
			Console.WriteLine("     |     |      ");
			Console.WriteLine("  {0}  |  {1}  |  {2}", tabuleiro[2,0], tabuleiro[2,1], tabuleiro[2,2]);
			Console.WriteLine("     |     |      ");
			Console.WriteLine("----------------------------------------------------");
			Console.WriteLine(" ");
		}
		
		public static void instrucoes()
		{
			//variavel local - não é o mesmo que guarda as jogadas
			char[][] tabuleiro = new char[3][];
			tabuleiro[0] = new char[3] {'1','2','3'};
			tabuleiro[1] = new char[3] {'4','5','6'};
			tabuleiro[2] = new char[3] {'7','8','9'};
			
			Console.ForegroundColor = ConsoleColor.Magenta;  //muda cor do texto
			Console.WriteLine("Utilize o teclado numerico para ecolher a casa no tabuleiro");
			Console.WriteLine(" Posições válidas: 1,2,3,4,5,6,7,8,9.\n");
			
			Console.WriteLine("     |     |      ");
			Console.WriteLine("  {0}  |  {1}  |  {2}", tabuleiro[0][0], tabuleiro[0][1], tabuleiro[0][2]);
			Console.WriteLine("_____|_____|_____ ");
			Console.WriteLine("     |     |      ");
			Console.WriteLine("  {0}  |  {1}  |  {2}", tabuleiro[1][0], tabuleiro[1][1], tabuleiro[1][2]);
			Console.WriteLine("_____|_____|_____ ");
			Console.WriteLine("     |     |      ");
			Console.WriteLine("  {0}  |  {1}  |  {2}", tabuleiro[2][0], tabuleiro[2][1], tabuleiro[2][2]);
			Console.WriteLine("     |     |      ");
			Console.WriteLine(" ");
			
			Console.WriteLine("Pressione uma tecla para inciar o jogo");
			Console.ReadKey(true);
			Console.Clear();
		}
		
		public static void mostra_resultado( Pessoa p )
		{
			Console.Clear();
			Console.WriteLine(" -------------------------- FIM  --------------------------");
			Console.WriteLine("Pessoa ganhadora: " + p.get_nome() + " / Simbolo:  " + p.get_simbolo());
		}
		
		public static void mostra_empate()
		{
			Console.Clear();
			Console.WriteLine(" -------------------------- FIM  --------------------------");
			Console.WriteLine("------------------------ EMPATE ------------------------");
		}
	}
}
