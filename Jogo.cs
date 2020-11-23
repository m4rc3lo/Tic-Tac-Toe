/*
 * User: m4rc3lo
 * Date: 12/10/2019
 * Time: 01:31
 */
using System;
namespace TTT
{
	//Classe para execucao e controle do jogo
	public class Jogo
	{
		// variável de instância (tem visibilidade em toda a classe)
		// abstrai o tabuleiro, tipo de dado: matrix de char
		private char[ , ] tabuleiro;
		
		private Pessoa pessoa1; 
		private Pessoa pessoa2;
		private char vencedor;
		
		//construtor da classe
		public Jogo()
		{
			//inicializacao da variavel (espaco em memoria)			
			tabuleiro = new char [3 , 3];
			pessoa1 = new Pessoa();
			pessoa2 = new Pessoa();
			vencedor = '-';
			
			//percorre cada posicao (linha x coluna)
			for (int linha = 0 ; linha < 3 ; linha++)
			{
				for (int coluna = 0 ; coluna < 3 ; coluna++)
				{
					tabuleiro [linha , coluna] = '-';
				}
			}
		}
		
		//sorteia qual pesso inicia o jogo (recebe X)
		public void sorteia_first ()
		{
			Random rand = new Random();
			int first = rand.Next(1,3);
			
			if (first == 1) 
			{
				pessoa1.set_simbolo('X');
				pessoa2.set_simbolo('O');
				pessoa1.set_first(true);
				pessoa1.set_turno(true);
			}
			else
			{
				pessoa1.set_simbolo('O');
				pessoa2.set_simbolo('X');
				pessoa2.set_first(true);
				pessoa2.set_turno(true);
			}
		}
		
		//nome e sorteio da primeira a jogar
		public void inicializa_game()
		{
			//define o nome das pessoas
			Desenho.apresentacao();
			Entrada.get_nomes(pessoa1, pessoa2);
			sorteia_first();//escolhe primeira pessoa a jogar
			//a primeira fica com o X
			Desenho.mostra_first(pessoa1,  pessoa2);
			Desenho.instrucoes();
		}
		
		//game loop - enquanto for possível jogar...
		public void start_game()
		{
			//variáveis locais a fução
			bool empate  = false;
			bool vitoria = false;
			//char vencedor = '-';
			int contador = 0;
			
			//enquanto não for vitória ou não for empate
			while (!(vitoria) && !(empate))
			{
				//variáveis locais ao escopo do while
				char eh_vitoria;
				int posicao = 0;
				
				//um jogador por turno
				if (pessoa1.get_turno())
				{
					Desenho.desenha_tabuleiro(tabuleiro);
					//pega a posicao da jogada escolhida
					posicao = Entrada.pega_jogada(pessoa1, tabuleiro);
					//passa o turno
					pessoa1.set_turno(false);
					pessoa2.set_turno(true);					
					Atualizacao.set_jogada(pessoa1, tabuleiro, posicao);
					eh_vitoria = Atualizacao.eh_vitoria(tabuleiro);
					Console.Clear();
					//caso alguem ganhe
					if (eh_vitoria == 'X')
					{		
						vitoria = true;
						vencedor = 'X';
					}
					else if (eh_vitoria == 'O')
					{
						vitoria = true;
						vencedor = 'O';
					}
					contador++; //controla quantidade de jogadas
				}
				else if (pessoa2.get_turno())
				{
					Desenho.desenha_tabuleiro(tabuleiro);
					posicao = Entrada.pega_jogada(pessoa2, tabuleiro);
					pessoa2.set_turno(false);
					pessoa1.set_turno(true);
					Atualizacao.set_jogada(pessoa2, tabuleiro, posicao);
					eh_vitoria = Atualizacao.eh_vitoria(tabuleiro);
					Console.Clear();
					
					if (eh_vitoria == 'X')
					{
						vitoria = true;
						vencedor = 'X';
					}
					else if (eh_vitoria == 'O')
					{
						vitoria = true;
						vencedor = 'O';
					}
					contador++;
				}
				
				if (contador >=9) // se não houver vitoria até o nono turmo
					empate = true;
			}//fim do while (vitoria ou empate)
		}//fim da função star_game()
		
		//encerra a partida
		public void end_game()
		{
			//verifica vencedor
			if(vencedor == pessoa1.get_simbolo()) 
				Desenho.mostra_resultado(pessoa1);
			else if (vencedor == pessoa2.get_simbolo())
				Desenho.mostra_resultado(pessoa2);
			else
				Desenho.mostra_empate();
			//mostra tabuleiro final
			Desenho.desenha_tabuleiro(tabuleiro);
			Console.Write("Precione qualquel tecla para encerrar! . . . ");
			Console.ReadKey(true);
		}
	}//fim do escopo da classe Jogo
}//fim do namespace