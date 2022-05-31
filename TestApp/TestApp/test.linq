<Query Kind="Program">
  <RuntimeVersion>3.1</RuntimeVersion>
</Query>

namespace Def {
	class Person {
		public int Id {get;set;}
		public string Name {get;set;}
		public int Age {get;set;}
		public List<Address> Addresses {get;set;}
	}

	class Address {
		public int Id {get;set;}
		public string Title {get;set;}
		public int PersonId {get;set;}
	}

	class Program {
		
		public static void Main(){
		
			var people = new List<Person> {
				new Person { Id = 1, Name = "Dima", Age = 21 },
				new Person { Id = 2, Name = "Dima2", Age = 21 }
			};
			
			var addresses = new List<Address> {
				new Address { Id = 1, Title = "Def", PersonId = 1 },
				new Address { Id = 2, Title = "Def2", PersonId = 1 },
				new Address { Id = 3, Title = "Def3", PersonId = 1 },
				new Address { Id = 4, Title = "Def4", PersonId = 1 },
				new Address { Id = 5, Title = "Def5", PersonId = 0 },
				new Address { Id = 6, Title = "Def6", PersonId = 1 }
			};
			
			var resAuth1 = (from p in people 662
							join a in addresses
							on p.Id equals a.PersonId
							select new {
								Person = p,
								Address = a
							});
			
			var resAuth2 = (from p in people
							from a in addresses
							where p.Id == a.PersonId
							select new {
								Person = p,
								Address = a
							});
			
			Console.Write(resAuth1);
			Console.Write(resAuth2);
			return;
			
			var res = (from p in people
						join a in addresses on p.Id equals a.PersonId into q
						from a in q.DefaultIfEmpty()
						select new {
							Person = p,
							Address = a
						});
			
			var res3 = people.GroupJoin(addresses, p => p.Id, a => a.PersonId, (key, res) => new {key, res = res});
			
			var res2 = (from p in people
						select p)
						.SelectMany(p => from a in addresses
							where a.PersonId == p.Id
							select new {a});
			
			Console.Write(res);
			Console.Write(res3);
		}
	
	}
}
