21184-Projecto de Engenharia Informática (UAB - 2024/2025)


# Projecto de Cadastro de Cemitério

>Este projeto é um sistema de gestão de cemitérios que moderniza o processo de cadastro e planeamento. Ele centraliza informações geoespaciais, dados de concessões e documentação num ambiente unificado, resolvendo o problema de gestão fragmentada e desactualizada.

---

## 🚀 Visão Geral

Este projeto é um sistema de gestão de cadastro de cemitérios construído sobre uma arquitectura moderna e coesa. Ele integra funcionalidades de georreferenciação, gestão de dados, autenticação e gestão de documentos numa única plataforma.

### 🌟 Funcionalidades Principais

* **Autenticação e Permissões:** Sistema de login seguro com controle de acesso baseado em JWT.
* **Visualização e Interatividade:** Mapa interativo que suporta navegação, medição de distâncias/áreas e visualização em múltiplos sistemas de referência de coordenadas (SRCs).
* **Gestão de Cadastro:** Ferramentas CRUD (Criar, Ler, Atualizar, Apagar) para gerir registos de [objetos de cadastro, por exemplo: construções, talhões].
* **Georreferenciação:** Importação de ficheiros geoespaciais (`.shp`, `.dwg`, etc.) e actualização da geometria de registos directamente no mapa.
* **Gestão Documental:** Associação de documentos (`.pdf`, `.jpg`) a registos de cadastro para uma gestão de ficheiros centralizada.

---

## 🛠️ Tecnologias Utilizadas

### Frontend
* **React:** Biblioteca para a construção da interface de utilizador.
* **Redux:** Ferramenta de gestão de estado global para aplicações complexas.
* **MapGuide React Layout:** Framework para a construção de interfaces de mapa interativas.
* **DHTMLX Suite:** Conjunto de componentes UI para janelas, menus e grelhas.
* **OpenLayers:** Biblioteca JavaScript para mapas dinâmicos.

### Backend
* **ASP.NET Core:** Framework para o desenvolvimento das Web APIs.
* **Entity Framework Core:** ORM para a comunicação com a base de dados.
* **PostgreSQL/PostGIS:** Base de dados relacional com extensões geoespaciais.
* **NetTopologySuite:** Biblioteca para manipulação e análise de geometrias.
* **JWT (JSON Web Tokens):** Padrão de segurança para autenticação.

---

## ⚙️ Instalação e Configuração

Para configurar e executar o projeto localmente, siga estes passos.

### Pré-requisitos
* .NET SDK
* aspnetcore-runtime-8.0.15-win-x64, dotnet-hosting-8.0.15-win
* Postgresql-17.4-1-windows-x64 com PostGis
* MapGuideOpenSource-4.0.0.10048-x64

### Backend Projecto .NET Visual Studio 2022
1.  Clone o repositório do projeto.
2.  Navegue até o diretório do `ApiServices`.
3.  Instale as dependências: `dotnet restore`
4.  Configure a base de dados [PostgreSQL/PostGIS]. Actualize a string de conexão no `appsettings.json`.
5.  Execute os scripts da base de dados localizados no directório: `BaseDados`
6.  Inicie a API: `dotnet run`

### Frontend Projecto Visual Studio Code
1.  Navegue até o diretório do `MVP_PCC`.
2.  Instale as dependências: `npm install`
3.  Configure as URLs das APIs no ficheiro `src/config.json`.
4.  Inicie a aplicação React: `npm start`


**José Augusto Azevedo - Nº 2200655**