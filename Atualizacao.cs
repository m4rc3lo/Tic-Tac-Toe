/* 
 * User: m4rc3lo
 * Date: 12/10/2019 
 */
using System;
namespace TTT
{
	// classe de controle auxiliar para o jogo
	public static class Atualizacao
	{
		//verifica posição livre
		public static bool eh_livre(char[,] t, int n)
		{
			bool resposta = false;
			switch (n)
			{
				case 1:
					if (t[0,0] == '-')
						resposta = true;
					break;
				
				case 2:
					if (t[0,1] == '-')
						resposta = true;
					break;
				case 3:
					if (t[0,2] == '-')
						resposta = true;
					break;
				case 4:
					if (t[1,0] == '-')
						resposta = true;
					break;
				case 5:
					if (t[1,1] == '-')
						resposta = true;
					break;
				case 6:
					if (t[1,2] == '-')
						resposta = true;
					break;
				case 7:
					if (t[2,0] == '-')
						resposta = true;
					break;
				case 8:
					if (t[2,1] == '-')
						resposta = true;
					break;
				case 9:
					if (t[2,2] == '-')
						resposta = true;
					break;
			}
			return resposta;
		}

		//verifica vitória
		public static char eh_vitoria(char[,] t)
		{
			char x = 'X', o = 'O', e = '-';
			int contx = 0;
			int conty = 0;

			//linha
			for (int linha = 0 ; linha < 3 ; linha++ ) 
			{
				if ((t[linha,0] == x) && (t[linha,1] == x) && (t[linha,2] == x))
					contx++;
				else if ((t[linha,0] == o) && (t[linha,1] == o) && (t[linha,2] == o))
					conty++;
			}
			
			//coluna
			for (int coluna = 0 ; coluna < 3 ; coluna++ ) 
			{
				if ((t[0, coluna] == x) && (t[1, coluna] == x) && (t[2,coluna] == x))
					contx++;
				else if ((t[0,coluna] == o) && (t[1,coluna] == o) && (t[2,coluna] == o))
					conty++;
			}
			
			//diagonais
			if ((t[0,0] == x) && (t[1,1] == x) && (t[2,2] == x))
				contx++;
			else if ((t[2,0] == x) && (t[1,1] == x) && (t[0,2] == x))
				contx++;
			else if ((t[0,0] == o) && (t[1,1] == o) && (t[2,2] == o))
				conty++;
			else if ((t[2,0] == o) && (t[1,1] == o) && (t[0,2] == o))
				conty++;

			if (contx > 0)
				return x;
			else if (conty > 0)
				return o;
			else
				return e;
		}

		// efetiva altereação na representação do tabuleiro
		public static void set_jogada(Pessoa j, char[,] t, int p)
		{
			switch (p)
			{
				case 1:
					t[0,0] = j.get_simbolo();
					break;
				case 2:
					t[0,1] = j.get_simbolo();
					break;
				case 3:
					t[0,2] = j.get_simbolo();
					break;
				case 4:
					t[1,0] = j.get_simbolo();
					break;
				case 5:
					t[1,1] = j.get_simbolo();
					break;
				case 6:
					t[1,2] = j.get_simbolo();
					break;
				case 7:
					t[2,0] = j.get_simbolo();
					break;
				case 8:
					t[2,1] = j.get_simbolo();
					break;
				case 9:
					t[2,2] = j.get_simbolo();
					break;
			}
		}
	}
}
