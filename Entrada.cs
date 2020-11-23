/*
 * User: m4rc3lo
 * Date: 13/10/2019
 * Time: 10:43
 */
using System;
namespace TTT
{
	//classe para receber as entradas de dados
	public class Entrada
	{
		//construtuor vazio
		public Entrada() { }
		
		//seta nome das pessoas jogando
		public static void get_nomes(Pessoa j1, Pessoa j2 )
		{
			//variavel local (escopo da funcao / nao da classe)
			string nome_temp = "";
			Console.WriteLine(" ");
			Console.WriteLine("Digite o nome 1: ");
			nome_temp =Console.ReadLine();
			j1.set_nome(nome_temp); //acessa mebro do objeto de tipo Pessoa
			
			Console.WriteLine("Digite o nome 2: ");
			nome_temp = Console.ReadLine();
			j2.set_nome(nome_temp);
		}

		//garante uma jogada válida
		//retorna a posição no tabuleiro
		public static int pega_jogada(Pessoa j, char [,] t)
		{
			//variáveis locais
			string jogada;
			int valor;
			bool valor_ok = false;
			bool livre_ok = false;
			bool controle = true;

			do // repeticão 
			{	
				Console.WriteLine(j.get_nome() + ": digitite sua jogada");
				jogada = Console.ReadLine();
				valor = Convert.ToInt16(jogada); //converte para inteiro

				if (valor >= 1 && valor <= 9) // testa numero válido
				{
					valor_ok = true;
					if (Atualizacao.eh_livre(t, valor)) //testa posição livre
						livre_ok = true;
					else
						Console.WriteLine("Digite uma posição desocupada");					
				}
				else
					Console.WriteLine("Digite um valor de posição válida");

				if (valor_ok && livre_ok) 
					controle = false;

			} while (controle); // fim da repetição
			return valor; //retorna uma movimento válido
		}
	}
}
