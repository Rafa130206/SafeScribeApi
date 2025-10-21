# Projeto SafeScribe

## Integrantes

- **André Luís Mesquita de Abreu** – RM: 558159  
- **Maria Eduarda Brigidio** – RM: 558575  
- **Rafael Bompadre Lima** – RM: 556459

## Descrição do Projeto

Este projeto é uma aplicação de gerenciamento de notas com autenticação via JWT, controle de acesso por roles e CRUD completo para notas.

## Funcionalidades

- Cadastro e login de usuários
- Criação, leitura, atualização e exclusão de notas
- Controle de acesso baseado em roles (Admin, Editor, Leitor)
- Retorno seguro de informações do usuário sem expor senhas

## Autenticação via Swagger

Como não há front-end, é necessário autenticar manualmente no Swagger:

1. Faça login usando o endpoint `/api/auth/login` para obter um token JWT.
2. Copie apenas o token enviado como resposta.
3. Clique no botão **Authorize** no Swagger.
4. Cole o token no campo `Value`.

Apenas com a realização desse passo o crud de notas funcionará devidamente!

