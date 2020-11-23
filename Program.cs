/*
 * User: m4rc3lo
 * Date: 11/10/2019
 * Time: 14:49
 */
using System;

// organizacao e limitacaode escopo
namespace TTT
{
	//classe que define a funcao principal 
	class Program
	{
		//ponto de partida do programa
		public static void Main(string[] args)
		{
			//-------------------------------------------------------------------
			//declaracao de variaveis (objetos)
			Jogo obj_jogo = new Jogo();
			//-------------------------------------------------------------------
			//execucao do jogo
			obj_jogo.inicializa_game();
			obj_jogo.start_game();
			obj_jogo.end_game();
			//-------------------------------------------------------------------
			//finalizando execucao
			//Console.ForegroundColor = ConsoleColor.Magenta;
					
			//-------------------------------------------------------------------
		}// quanto esta funcao encerra o programa acaba
	}//fim da declaracao da classe
}//fim da declaracao de namespace