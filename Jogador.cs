/*
 * User: m4rc3lo
 * Date: 13/10/2019
 * Time: 10:39
 */
using System;
namespace TTT
{
	//classe para representar uma pessoa jogando
	public class Pessoa
	{
		//variaveis de instancia
		private string nome;
		private char simbolo;
		private bool first;
		private bool turno;
		
		//construtor
		public Pessoa()
		{
			nome = " ";
			simbolo = ' ';
			first = false;
			turno = false;
		}
		//acesso as variaveis de isntancia
		public void set_nome(string n)
		{
			nome = n;
		}
		public void set_simbolo(char s)
		{
			simbolo = s;
		}
		public void set_turno(bool t)
		{
			turno = t;
		}
		public void set_first(bool f)
		{
			first = f;
		}
		public char get_simbolo()
		{
			return simbolo;
		}
		public string get_nome()
		{
			return nome;
		}
		public bool get_turno()
		{
			return turno;
		}
		public bool get_first()
		{
			return first;
		}
	}
}
