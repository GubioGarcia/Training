# Análise Inicial do Projeto

Desenvolvimento de uma plataforma WEB que reúne funcionalidades para profissionais da área de educação física, permitindo a gestão de informações sobre seus alunos. Além disso, a plataforma possibilita o acompanhamento de treinos (musculação, funcional e cross) ofertados ao aluno. O aluno, por sua vez, poderá acessar seu perfil e obter as atualizações do personal, registrando os dias de execução dos treinos e informações como cargas usadas, séries e repetições efetuadas.

---

## Informações Importantes

### Cliente Direto

- Profissionais de Educação Física que trabalham com consultoria online

### Stakeholders - Partes Interessadas

- Profissionais de Educação Física
- Alunos

### Benefícios

- **Personal:** Controle do quadro de alunos, montagem e atualização de treinos, controle de frequência do aluno, feedback dos alunos, aumento na exposição comercial, disponibilização de material de apoio ao aluno. Tudo isso integrado em uma única plataforma.
- **Aluno:** Plataforma individualizada para registro de treinos, acompanhamento do protocolo individual, acesso a conteúdos de apoio disponibilizados pelo professor.

### Produto

- Plataforma web

---

## Funcionalidades do Sistema

- **Login (Personal e Aluno):**
  - Cadastro e ativação do personal feito pelo admin;
  - Cadastro e ativação do aluno feito pelo personal;
  - Login realizado com CPF e senha.

- **Gestão de Aluno (pelo Personal):**
  - Inserir, editar e remover alunos:
    - Limitado às informações de cadastro, treinos e objetivos do aluno;
    - Demais funções são responsabilidade do aluno;
  - Definir se o aluno está ativo ou não:
    - A definição inibe o acesso do aluno ao sistema.

- **Gestão de Perfil:**
  - Página de perfil com informações adicionais:
    - Foto;
    - Nome;
    - Idade;
    - Data de nascimento;
    - Tipo de perfil (aluno ou personal);
    - Tipo de treino;
    - Objetivo;
    - Altura;
    - Peso inicial e peso atual.

- **Gestão de Treinos (pelo Personal):**
  - Inserir, editar e remover treinos por aluno;
  - Definição do tipo de treino (Musculação, Funcional ou Cross);
  - Montagem do treino:
    - Definição do período do treino (data de início e fim);
    - Escolha da rotina de treino:
      - Divisão dos treinos (ABC, ABCDE, ou nomeação de cada treino na periodização);
      - Periodicidade dos treinos;
    - Escolha dos exercícios para cada treino;
    - Escolha da quantidade de séries;
    - Escolha da quantidade de repetições;
    - Escolha do intervalo entre séries;
  - Disponibilização de informações e orientações.

- **Execução de Treinos (pelo Aluno):**
  - Registro de execução das séries;
  - Registro de cargas usadas;
  - Indicação de informações/observações por treino.
