# api.Net8

API em **.NET 8** com migrações (Entity Framework).

---

## 🧩 Tecnologias e stack

- .NET 8  
- C#  
- Entity Framework Core (Code First / Migrations)  
- Injeção de dependência (DI)  
- Camadas: Core, Infrastructure, Services  

---

## 🗂 Estrutura do projeto
├─ Core/ ← Entidades, interfaces, regras de negócio
├─ Infrastructure/ ← Contexto do banco, migrations, implementações de repositório
├─ Services/ ← Lógica de aplicação / casos de uso

## ⚙️ Como rodar / executar

1. Clone o repositório  
   ```bash
   git clone https://github.com/viniciudev/api.Net8.git

2.Entre na pasta do projeto
cd api.Net8


3.Verifique se você tem o SDK .NET 8 instalado
dotnet --version


4.Crie / atualize banco de dados com migrações
dotnet ef database update --project Infrastructure --startup-project api.Net8


5.Execute a API
dotnet run --project api.Net8


Acesse no navegador ou via cliente HTTP

http://localhost:5000/swagger  
ou  
http://localhost:5000/api/[controller]
