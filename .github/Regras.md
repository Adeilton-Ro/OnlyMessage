# Regras de Negócio

## Registro
Cria a conta do usuário com suas credenciais(username e senha), a senha será salva em hash256 com um salt para maior segurança das senhas.

## Login
Usa as credenciais anteriormente registradas para retornar um Token, que pode ser usado para se autenticar, e o tempo de expiração do mesmo.

## Enviar solicitação
Busca a pessoa pelo Id, salva no banco o relacionamento de solicitação de amizades, depois envia a solicitação de amizade.

## Retornar usuários
Retorna todos os usuários que estam ná aplicação com o Id e username.

## Aceitar solicitação
Permite que o usuário aceite solicitações pendentes. Quando a ação for feita a API deve apagar a solicitação do Banco, usando o id da solicitação, e adicionar o relacionamento de amigos no banco.

## Remover Amigo
Permite apagar o relacionamento entre os usuários, removendo-o de suas listas de amigos.

## Listar Amigos
Retorna os amigos que o usuário possui com as fotos, ultima mensagem e a data, com hora, dessa ultima mensagem.

## Listar Grupos
Retorna os grupos que o usuário está inserido com as fotos, ultima mensagem e a data, com hora, dessa ultima mensagem.

## Pegar Mensagens de um amigo/grupo
Retorna as mensagens de um amigo/grupo em especifico, junto com as informações daquele usuário/grupo.

## Enviar Mensagem
Permite enviar mensagem para uma pessoa ou grupo enviando o id e a mensagem.

## Criar Grupo
Permite que um usuário crie um grupo com um nome e uma descrição.

## Sair de um Grupo
Permite que o usuário não receba mensagens de um grupo que estava inserido. Para ser removido de um grupo é necessario enviar o id do grupo que deseja sair.

## Criar link
Permite que o usuário gere um link para enviar para seus amigos entrarem em seu grupo.