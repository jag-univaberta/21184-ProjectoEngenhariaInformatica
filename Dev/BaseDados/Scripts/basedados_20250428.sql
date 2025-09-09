--
-- PostgreSQL database dump
--

-- Dumped from database version 17.4
-- Dumped by pg_dump version 17.4

-- Started on 2025-04-28 09:51:52

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 8 (class 2615 OID 19199)
-- Name: acesso; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA acesso;


ALTER SCHEMA acesso OWNER TO postgres;

--
-- TOC entry 7 (class 2615 OID 19201)
-- Name: cadastro; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA cadastro;


ALTER SCHEMA cadastro OWNER TO postgres;

--
-- TOC entry 9 (class 2615 OID 20502)
-- Name: inf_geografica; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA inf_geografica;


ALTER SCHEMA inf_geografica OWNER TO postgres;

--
-- TOC entry 2 (class 3079 OID 19318)
-- Name: postgis; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS postgis WITH SCHEMA public;


--
-- TOC entry 5949 (class 0 OID 0)
-- Dependencies: 2
-- Name: EXTENSION postgis; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION postgis IS 'PostGIS geometry and geography spatial types and functions';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 227 (class 1259 OID 19305)
-- Name: funcionalidades; Type: TABLE; Schema: acesso; Owner: postgres
--

CREATE TABLE acesso.funcionalidades (
    codigo_funcionalidade integer NOT NULL,
    designacao character varying(100) NOT NULL,
    codigo_pai integer
);


ALTER TABLE acesso.funcionalidades OWNER TO postgres;

--
-- TOC entry 222 (class 1259 OID 19283)
-- Name: grupos; Type: TABLE; Schema: acesso; Owner: postgres
--

CREATE TABLE acesso.grupos (
    rec_id integer NOT NULL,
    nome character varying(100) NOT NULL,
    activo boolean DEFAULT true
);


ALTER TABLE acesso.grupos OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 19282)
-- Name: grupos_rec_id_seq; Type: SEQUENCE; Schema: acesso; Owner: postgres
--

CREATE SEQUENCE acesso.grupos_rec_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE acesso.grupos_rec_id_seq OWNER TO postgres;

--
-- TOC entry 5950 (class 0 OID 0)
-- Dependencies: 221
-- Name: grupos_rec_id_seq; Type: SEQUENCE OWNED BY; Schema: acesso; Owner: postgres
--

ALTER SEQUENCE acesso.grupos_rec_id_seq OWNED BY acesso.grupos.rec_id;


--
-- TOC entry 226 (class 1259 OID 19299)
-- Name: grupos_utilizadores; Type: TABLE; Schema: acesso; Owner: postgres
--

CREATE TABLE acesso.grupos_utilizadores (
    rec_id integer NOT NULL,
    utilizador_id integer NOT NULL,
    grupo_id integer NOT NULL
);


ALTER TABLE acesso.grupos_utilizadores OWNER TO postgres;

--
-- TOC entry 225 (class 1259 OID 19298)
-- Name: grupos_utilizadores_rec_id_seq; Type: SEQUENCE; Schema: acesso; Owner: postgres
--

CREATE SEQUENCE acesso.grupos_utilizadores_rec_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE acesso.grupos_utilizadores_rec_id_seq OWNER TO postgres;

--
-- TOC entry 5951 (class 0 OID 0)
-- Dependencies: 225
-- Name: grupos_utilizadores_rec_id_seq; Type: SEQUENCE OWNED BY; Schema: acesso; Owner: postgres
--

ALTER SEQUENCE acesso.grupos_utilizadores_rec_id_seq OWNED BY acesso.grupos_utilizadores.rec_id;


--
-- TOC entry 229 (class 1259 OID 19311)
-- Name: permissoes; Type: TABLE; Schema: acesso; Owner: postgres
--

CREATE TABLE acesso.permissoes (
    rec_id integer NOT NULL,
    utilizador_id integer NOT NULL,
    grupo_id integer NOT NULL,
    funcionalidade_id integer NOT NULL,
    permissao boolean DEFAULT true
);


ALTER TABLE acesso.permissoes OWNER TO postgres;

--
-- TOC entry 228 (class 1259 OID 19310)
-- Name: permissoes_rec_id_seq; Type: SEQUENCE; Schema: acesso; Owner: postgres
--

CREATE SEQUENCE acesso.permissoes_rec_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE acesso.permissoes_rec_id_seq OWNER TO postgres;

--
-- TOC entry 5952 (class 0 OID 0)
-- Dependencies: 228
-- Name: permissoes_rec_id_seq; Type: SEQUENCE OWNED BY; Schema: acesso; Owner: postgres
--

ALTER SEQUENCE acesso.permissoes_rec_id_seq OWNED BY acesso.permissoes.rec_id;


--
-- TOC entry 224 (class 1259 OID 19291)
-- Name: utilizadores; Type: TABLE; Schema: acesso; Owner: postgres
--

CREATE TABLE acesso.utilizadores (
    rec_id integer NOT NULL,
    nome character varying(100) NOT NULL,
    utilizador character varying(100) NOT NULL,
    palavra_passe character varying(100) NOT NULL,
    email character varying(100) NOT NULL,
    activo boolean DEFAULT true
);


ALTER TABLE acesso.utilizadores OWNER TO postgres;

--
-- TOC entry 223 (class 1259 OID 19290)
-- Name: utilizadores_rec_id_seq; Type: SEQUENCE; Schema: acesso; Owner: postgres
--

CREATE SEQUENCE acesso.utilizadores_rec_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE acesso.utilizadores_rec_id_seq OWNER TO postgres;

--
-- TOC entry 5953 (class 0 OID 0)
-- Dependencies: 223
-- Name: utilizadores_rec_id_seq; Type: SEQUENCE OWNED BY; Schema: acesso; Owner: postgres
--

ALTER SEQUENCE acesso.utilizadores_rec_id_seq OWNED BY acesso.utilizadores.rec_id;


--
-- TOC entry 238 (class 1259 OID 20427)
-- Name: cemiterios; Type: TABLE; Schema: cadastro; Owner: postgres
--

CREATE TABLE cadastro.cemiterios (
    rec_id integer NOT NULL,
    nome character varying(100) NOT NULL,
    morada character varying(100) NOT NULL,
    dicofre character varying(100) NOT NULL
);


ALTER TABLE cadastro.cemiterios OWNER TO postgres;

--
-- TOC entry 237 (class 1259 OID 20426)
-- Name: cemiterios_rec_id_seq; Type: SEQUENCE; Schema: cadastro; Owner: postgres
--

CREATE SEQUENCE cadastro.cemiterios_rec_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE cadastro.cemiterios_rec_id_seq OWNER TO postgres;

--
-- TOC entry 5954 (class 0 OID 0)
-- Dependencies: 237
-- Name: cemiterios_rec_id_seq; Type: SEQUENCE OWNED BY; Schema: cadastro; Owner: postgres
--

ALTER SEQUENCE cadastro.cemiterios_rec_id_seq OWNED BY cadastro.cemiterios.rec_id;


--
-- TOC entry 244 (class 1259 OID 20451)
-- Name: concessionarios; Type: TABLE; Schema: cadastro; Owner: postgres
--

CREATE TABLE cadastro.concessionarios (
    rec_id integer NOT NULL,
    nome character varying(100) NOT NULL,
    morada character varying(100) NOT NULL,
    dicofre character varying(100) NOT NULL,
    contacto character varying(100) NOT NULL
);


ALTER TABLE cadastro.concessionarios OWNER TO postgres;

--
-- TOC entry 243 (class 1259 OID 20450)
-- Name: concessionarios_rec_id_seq; Type: SEQUENCE; Schema: cadastro; Owner: postgres
--

CREATE SEQUENCE cadastro.concessionarios_rec_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE cadastro.concessionarios_rec_id_seq OWNER TO postgres;

--
-- TOC entry 5955 (class 0 OID 0)
-- Dependencies: 243
-- Name: concessionarios_rec_id_seq; Type: SEQUENCE OWNED BY; Schema: cadastro; Owner: postgres
--

ALTER SEQUENCE cadastro.concessionarios_rec_id_seq OWNED BY cadastro.concessionarios.rec_id;


--
-- TOC entry 236 (class 1259 OID 20417)
-- Name: construcoes; Type: TABLE; Schema: cadastro; Owner: postgres
--

CREATE TABLE cadastro.construcoes (
    rec_id integer NOT NULL,
    tipoconstrucao_id integer NOT NULL,
    designacao character varying(100) NOT NULL,
    talhao_id integer NOT NULL,
    geometria public.geometry
);


ALTER TABLE cadastro.construcoes OWNER TO postgres;

--
-- TOC entry 246 (class 1259 OID 20458)
-- Name: construcoes_concessionarios; Type: TABLE; Schema: cadastro; Owner: postgres
--

CREATE TABLE cadastro.construcoes_concessionarios (
    rec_id integer NOT NULL,
    concessionario_id integer NOT NULL,
    construcao_id integer NOT NULL,
    data_inicio character varying(8) NOT NULL,
    data_fim character varying(8)
);


ALTER TABLE cadastro.construcoes_concessionarios OWNER TO postgres;

--
-- TOC entry 245 (class 1259 OID 20457)
-- Name: construcoes_concessionarios_rec_id_seq; Type: SEQUENCE; Schema: cadastro; Owner: postgres
--

CREATE SEQUENCE cadastro.construcoes_concessionarios_rec_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE cadastro.construcoes_concessionarios_rec_id_seq OWNER TO postgres;

--
-- TOC entry 5956 (class 0 OID 0)
-- Dependencies: 245
-- Name: construcoes_concessionarios_rec_id_seq; Type: SEQUENCE OWNED BY; Schema: cadastro; Owner: postgres
--

ALTER SEQUENCE cadastro.construcoes_concessionarios_rec_id_seq OWNED BY cadastro.construcoes_concessionarios.rec_id;


--
-- TOC entry 235 (class 1259 OID 20416)
-- Name: construcoes_rec_id_seq; Type: SEQUENCE; Schema: cadastro; Owner: postgres
--

CREATE SEQUENCE cadastro.construcoes_rec_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE cadastro.construcoes_rec_id_seq OWNER TO postgres;

--
-- TOC entry 5957 (class 0 OID 0)
-- Dependencies: 235
-- Name: construcoes_rec_id_seq; Type: SEQUENCE OWNED BY; Schema: cadastro; Owner: postgres
--

ALTER SEQUENCE cadastro.construcoes_rec_id_seq OWNED BY cadastro.construcoes.rec_id;


--
-- TOC entry 254 (class 1259 OID 20496)
-- Name: ficheiros_associados; Type: TABLE; Schema: cadastro; Owner: postgres
--

CREATE TABLE cadastro.ficheiros_associados (
    rec_id integer NOT NULL,
    nome_documento character varying(100) NOT NULL,
    nome_atribuido character varying(64) NOT NULL,
    descricao_documento character varying(100) NOT NULL,
    datahoraupload character varying(14),
    tipo_associacao character varying(20),
    codigo_associacao integer NOT NULL
);


ALTER TABLE cadastro.ficheiros_associados OWNER TO postgres;

--
-- TOC entry 253 (class 1259 OID 20495)
-- Name: ficheiros_associados_rec_id_seq; Type: SEQUENCE; Schema: cadastro; Owner: postgres
--

CREATE SEQUENCE cadastro.ficheiros_associados_rec_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE cadastro.ficheiros_associados_rec_id_seq OWNER TO postgres;

--
-- TOC entry 5958 (class 0 OID 0)
-- Dependencies: 253
-- Name: ficheiros_associados_rec_id_seq; Type: SEQUENCE OWNED BY; Schema: cadastro; Owner: postgres
--

ALTER SEQUENCE cadastro.ficheiros_associados_rec_id_seq OWNED BY cadastro.ficheiros_associados.rec_id;


--
-- TOC entry 252 (class 1259 OID 20479)
-- Name: movimentos; Type: TABLE; Schema: cadastro; Owner: postgres
--

CREATE TABLE cadastro.movimentos (
    rec_id integer NOT NULL,
    data_movimento character varying(8) NOT NULL,
    residente_id integer NOT NULL,
    tipomovimento_id integer NOT NULL,
    construcaodestino_id integer NOT NULL
);


ALTER TABLE cadastro.movimentos OWNER TO postgres;

--
-- TOC entry 251 (class 1259 OID 20478)
-- Name: movimentos_rec_id_seq; Type: SEQUENCE; Schema: cadastro; Owner: postgres
--

CREATE SEQUENCE cadastro.movimentos_rec_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE cadastro.movimentos_rec_id_seq OWNER TO postgres;

--
-- TOC entry 5959 (class 0 OID 0)
-- Dependencies: 251
-- Name: movimentos_rec_id_seq; Type: SEQUENCE OWNED BY; Schema: cadastro; Owner: postgres
--

ALTER SEQUENCE cadastro.movimentos_rec_id_seq OWNED BY cadastro.movimentos.rec_id;


--
-- TOC entry 250 (class 1259 OID 20472)
-- Name: residentes; Type: TABLE; Schema: cadastro; Owner: postgres
--

CREATE TABLE cadastro.residentes (
    rec_id integer NOT NULL,
    nome character varying(100) NOT NULL,
    data_nascimento character varying(8),
    data_falecimento character varying(8),
    data_inumacao character varying(8)
);


ALTER TABLE cadastro.residentes OWNER TO postgres;

--
-- TOC entry 249 (class 1259 OID 20471)
-- Name: residentes_rec_id_seq; Type: SEQUENCE; Schema: cadastro; Owner: postgres
--

CREATE SEQUENCE cadastro.residentes_rec_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE cadastro.residentes_rec_id_seq OWNER TO postgres;

--
-- TOC entry 5960 (class 0 OID 0)
-- Dependencies: 249
-- Name: residentes_rec_id_seq; Type: SEQUENCE OWNED BY; Schema: cadastro; Owner: postgres
--

ALTER SEQUENCE cadastro.residentes_rec_id_seq OWNED BY cadastro.residentes.rec_id;


--
-- TOC entry 240 (class 1259 OID 20434)
-- Name: talhao; Type: TABLE; Schema: cadastro; Owner: postgres
--

CREATE TABLE cadastro.talhao (
    rec_id integer NOT NULL,
    codigo character varying(100) NOT NULL,
    cemiterio_id integer NOT NULL,
    geometria public.geometry
);


ALTER TABLE cadastro.talhao OWNER TO postgres;

--
-- TOC entry 239 (class 1259 OID 20433)
-- Name: talhao_rec_id_seq; Type: SEQUENCE; Schema: cadastro; Owner: postgres
--

CREATE SEQUENCE cadastro.talhao_rec_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE cadastro.talhao_rec_id_seq OWNER TO postgres;

--
-- TOC entry 5961 (class 0 OID 0)
-- Dependencies: 239
-- Name: talhao_rec_id_seq; Type: SEQUENCE OWNED BY; Schema: cadastro; Owner: postgres
--

ALTER SEQUENCE cadastro.talhao_rec_id_seq OWNED BY cadastro.talhao.rec_id;


--
-- TOC entry 242 (class 1259 OID 20444)
-- Name: tipos_construcao; Type: TABLE; Schema: cadastro; Owner: postgres
--

CREATE TABLE cadastro.tipos_construcao (
    rec_id integer NOT NULL,
    designacao character varying(100) NOT NULL
);


ALTER TABLE cadastro.tipos_construcao OWNER TO postgres;

--
-- TOC entry 241 (class 1259 OID 20443)
-- Name: tipos_construcao_rec_id_seq; Type: SEQUENCE; Schema: cadastro; Owner: postgres
--

CREATE SEQUENCE cadastro.tipos_construcao_rec_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE cadastro.tipos_construcao_rec_id_seq OWNER TO postgres;

--
-- TOC entry 5962 (class 0 OID 0)
-- Dependencies: 241
-- Name: tipos_construcao_rec_id_seq; Type: SEQUENCE OWNED BY; Schema: cadastro; Owner: postgres
--

ALTER SEQUENCE cadastro.tipos_construcao_rec_id_seq OWNED BY cadastro.tipos_construcao.rec_id;


--
-- TOC entry 248 (class 1259 OID 20465)
-- Name: tipos_movimentos; Type: TABLE; Schema: cadastro; Owner: postgres
--

CREATE TABLE cadastro.tipos_movimentos (
    rec_id integer NOT NULL,
    designacao character varying(100) NOT NULL
);


ALTER TABLE cadastro.tipos_movimentos OWNER TO postgres;

--
-- TOC entry 247 (class 1259 OID 20464)
-- Name: tipos_movimentos_rec_id_seq; Type: SEQUENCE; Schema: cadastro; Owner: postgres
--

CREATE SEQUENCE cadastro.tipos_movimentos_rec_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE cadastro.tipos_movimentos_rec_id_seq OWNER TO postgres;

--
-- TOC entry 5963 (class 0 OID 0)
-- Dependencies: 247
-- Name: tipos_movimentos_rec_id_seq; Type: SEQUENCE OWNED BY; Schema: cadastro; Owner: postgres
--

ALTER SEQUENCE cadastro.tipos_movimentos_rec_id_seq OWNED BY cadastro.tipos_movimentos.rec_id;


--
-- TOC entry 256 (class 1259 OID 20504)
-- Name: cartografia; Type: TABLE; Schema: inf_geografica; Owner: postgres
--

CREATE TABLE inf_geografica.cartografia (
    rec_id integer NOT NULL,
    nome character varying(256),
    parent integer,
    ordem integer
);


ALTER TABLE inf_geografica.cartografia OWNER TO postgres;

--
-- TOC entry 255 (class 1259 OID 20503)
-- Name: cartografia_rec_id_seq; Type: SEQUENCE; Schema: inf_geografica; Owner: postgres
--

CREATE SEQUENCE inf_geografica.cartografia_rec_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE inf_geografica.cartografia_rec_id_seq OWNER TO postgres;

--
-- TOC entry 5964 (class 0 OID 0)
-- Dependencies: 255
-- Name: cartografia_rec_id_seq; Type: SEQUENCE OWNED BY; Schema: inf_geografica; Owner: postgres
--

ALTER SEQUENCE inf_geografica.cartografia_rec_id_seq OWNED BY inf_geografica.cartografia.rec_id;


--
-- TOC entry 258 (class 1259 OID 20511)
-- Name: cartografialayers; Type: TABLE; Schema: inf_geografica; Owner: postgres
--

CREATE TABLE inf_geografica.cartografialayers (
    rec_id integer NOT NULL,
    parent integer NOT NULL,
    layer character varying(256)
);


ALTER TABLE inf_geografica.cartografialayers OWNER TO postgres;

--
-- TOC entry 257 (class 1259 OID 20510)
-- Name: cartografialayers_rec_id_seq; Type: SEQUENCE; Schema: inf_geografica; Owner: postgres
--

CREATE SEQUENCE inf_geografica.cartografialayers_rec_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE inf_geografica.cartografialayers_rec_id_seq OWNER TO postgres;

--
-- TOC entry 5965 (class 0 OID 0)
-- Dependencies: 257
-- Name: cartografialayers_rec_id_seq; Type: SEQUENCE OWNED BY; Schema: inf_geografica; Owner: postgres
--

ALTER SEQUENCE inf_geografica.cartografialayers_rec_id_seq OWNED BY inf_geografica.cartografialayers.rec_id;


--
-- TOC entry 5736 (class 2604 OID 19286)
-- Name: grupos rec_id; Type: DEFAULT; Schema: acesso; Owner: postgres
--

ALTER TABLE ONLY acesso.grupos ALTER COLUMN rec_id SET DEFAULT nextval('acesso.grupos_rec_id_seq'::regclass);


--
-- TOC entry 5740 (class 2604 OID 19302)
-- Name: grupos_utilizadores rec_id; Type: DEFAULT; Schema: acesso; Owner: postgres
--

ALTER TABLE ONLY acesso.grupos_utilizadores ALTER COLUMN rec_id SET DEFAULT nextval('acesso.grupos_utilizadores_rec_id_seq'::regclass);


--
-- TOC entry 5741 (class 2604 OID 19314)
-- Name: permissoes rec_id; Type: DEFAULT; Schema: acesso; Owner: postgres
--

ALTER TABLE ONLY acesso.permissoes ALTER COLUMN rec_id SET DEFAULT nextval('acesso.permissoes_rec_id_seq'::regclass);


--
-- TOC entry 5738 (class 2604 OID 19294)
-- Name: utilizadores rec_id; Type: DEFAULT; Schema: acesso; Owner: postgres
--

ALTER TABLE ONLY acesso.utilizadores ALTER COLUMN rec_id SET DEFAULT nextval('acesso.utilizadores_rec_id_seq'::regclass);


--
-- TOC entry 5744 (class 2604 OID 20430)
-- Name: cemiterios rec_id; Type: DEFAULT; Schema: cadastro; Owner: postgres
--

ALTER TABLE ONLY cadastro.cemiterios ALTER COLUMN rec_id SET DEFAULT nextval('cadastro.cemiterios_rec_id_seq'::regclass);


--
-- TOC entry 5747 (class 2604 OID 20454)
-- Name: concessionarios rec_id; Type: DEFAULT; Schema: cadastro; Owner: postgres
--

ALTER TABLE ONLY cadastro.concessionarios ALTER COLUMN rec_id SET DEFAULT nextval('cadastro.concessionarios_rec_id_seq'::regclass);


--
-- TOC entry 5743 (class 2604 OID 20420)
-- Name: construcoes rec_id; Type: DEFAULT; Schema: cadastro; Owner: postgres
--

ALTER TABLE ONLY cadastro.construcoes ALTER COLUMN rec_id SET DEFAULT nextval('cadastro.construcoes_rec_id_seq'::regclass);


--
-- TOC entry 5748 (class 2604 OID 20461)
-- Name: construcoes_concessionarios rec_id; Type: DEFAULT; Schema: cadastro; Owner: postgres
--

ALTER TABLE ONLY cadastro.construcoes_concessionarios ALTER COLUMN rec_id SET DEFAULT nextval('cadastro.construcoes_concessionarios_rec_id_seq'::regclass);


--
-- TOC entry 5752 (class 2604 OID 20499)
-- Name: ficheiros_associados rec_id; Type: DEFAULT; Schema: cadastro; Owner: postgres
--

ALTER TABLE ONLY cadastro.ficheiros_associados ALTER COLUMN rec_id SET DEFAULT nextval('cadastro.ficheiros_associados_rec_id_seq'::regclass);


--
-- TOC entry 5751 (class 2604 OID 20482)
-- Name: movimentos rec_id; Type: DEFAULT; Schema: cadastro; Owner: postgres
--

ALTER TABLE ONLY cadastro.movimentos ALTER COLUMN rec_id SET DEFAULT nextval('cadastro.movimentos_rec_id_seq'::regclass);


--
-- TOC entry 5750 (class 2604 OID 20475)
-- Name: residentes rec_id; Type: DEFAULT; Schema: cadastro; Owner: postgres
--

ALTER TABLE ONLY cadastro.residentes ALTER COLUMN rec_id SET DEFAULT nextval('cadastro.residentes_rec_id_seq'::regclass);


--
-- TOC entry 5745 (class 2604 OID 20437)
-- Name: talhao rec_id; Type: DEFAULT; Schema: cadastro; Owner: postgres
--

ALTER TABLE ONLY cadastro.talhao ALTER COLUMN rec_id SET DEFAULT nextval('cadastro.talhao_rec_id_seq'::regclass);


--
-- TOC entry 5746 (class 2604 OID 20447)
-- Name: tipos_construcao rec_id; Type: DEFAULT; Schema: cadastro; Owner: postgres
--

ALTER TABLE ONLY cadastro.tipos_construcao ALTER COLUMN rec_id SET DEFAULT nextval('cadastro.tipos_construcao_rec_id_seq'::regclass);


--
-- TOC entry 5749 (class 2604 OID 20468)
-- Name: tipos_movimentos rec_id; Type: DEFAULT; Schema: cadastro; Owner: postgres
--

ALTER TABLE ONLY cadastro.tipos_movimentos ALTER COLUMN rec_id SET DEFAULT nextval('cadastro.tipos_movimentos_rec_id_seq'::regclass);


--
-- TOC entry 5753 (class 2604 OID 20507)
-- Name: cartografia rec_id; Type: DEFAULT; Schema: inf_geografica; Owner: postgres
--

ALTER TABLE ONLY inf_geografica.cartografia ALTER COLUMN rec_id SET DEFAULT nextval('inf_geografica.cartografia_rec_id_seq'::regclass);


--
-- TOC entry 5754 (class 2604 OID 20514)
-- Name: cartografialayers rec_id; Type: DEFAULT; Schema: inf_geografica; Owner: postgres
--

ALTER TABLE ONLY inf_geografica.cartografialayers ALTER COLUMN rec_id SET DEFAULT nextval('inf_geografica.cartografialayers_rec_id_seq'::regclass);


--
-- TOC entry 5763 (class 2606 OID 19309)
-- Name: funcionalidades funcionalidades_pkey; Type: CONSTRAINT; Schema: acesso; Owner: postgres
--

ALTER TABLE ONLY acesso.funcionalidades
    ADD CONSTRAINT funcionalidades_pkey PRIMARY KEY (codigo_funcionalidade);


--
-- TOC entry 5757 (class 2606 OID 19289)
-- Name: grupos grupos_pkey; Type: CONSTRAINT; Schema: acesso; Owner: postgres
--

ALTER TABLE ONLY acesso.grupos
    ADD CONSTRAINT grupos_pkey PRIMARY KEY (rec_id);


--
-- TOC entry 5761 (class 2606 OID 19304)
-- Name: grupos_utilizadores grupos_utilizadores_pkey; Type: CONSTRAINT; Schema: acesso; Owner: postgres
--

ALTER TABLE ONLY acesso.grupos_utilizadores
    ADD CONSTRAINT grupos_utilizadores_pkey PRIMARY KEY (rec_id);


--
-- TOC entry 5765 (class 2606 OID 19317)
-- Name: permissoes permissoes_pkey; Type: CONSTRAINT; Schema: acesso; Owner: postgres
--

ALTER TABLE ONLY acesso.permissoes
    ADD CONSTRAINT permissoes_pkey PRIMARY KEY (rec_id);


--
-- TOC entry 5759 (class 2606 OID 19297)
-- Name: utilizadores utilizadores_pkey; Type: CONSTRAINT; Schema: acesso; Owner: postgres
--

ALTER TABLE ONLY acesso.utilizadores
    ADD CONSTRAINT utilizadores_pkey PRIMARY KEY (rec_id);


--
-- TOC entry 5772 (class 2606 OID 20432)
-- Name: cemiterios cemiterios_pkey; Type: CONSTRAINT; Schema: cadastro; Owner: postgres
--

ALTER TABLE ONLY cadastro.cemiterios
    ADD CONSTRAINT cemiterios_pkey PRIMARY KEY (rec_id);


--
-- TOC entry 5779 (class 2606 OID 20456)
-- Name: concessionarios concessionarios_pkey; Type: CONSTRAINT; Schema: cadastro; Owner: postgres
--

ALTER TABLE ONLY cadastro.concessionarios
    ADD CONSTRAINT concessionarios_pkey PRIMARY KEY (rec_id);


--
-- TOC entry 5781 (class 2606 OID 20463)
-- Name: construcoes_concessionarios construcoes_concessionarios_pkey; Type: CONSTRAINT; Schema: cadastro; Owner: postgres
--

ALTER TABLE ONLY cadastro.construcoes_concessionarios
    ADD CONSTRAINT construcoes_concessionarios_pkey PRIMARY KEY (rec_id);


--
-- TOC entry 5770 (class 2606 OID 20424)
-- Name: construcoes construcoes_pkey; Type: CONSTRAINT; Schema: cadastro; Owner: postgres
--

ALTER TABLE ONLY cadastro.construcoes
    ADD CONSTRAINT construcoes_pkey PRIMARY KEY (rec_id);


--
-- TOC entry 5789 (class 2606 OID 20501)
-- Name: ficheiros_associados ficheiros_associados_pkey; Type: CONSTRAINT; Schema: cadastro; Owner: postgres
--

ALTER TABLE ONLY cadastro.ficheiros_associados
    ADD CONSTRAINT ficheiros_associados_pkey PRIMARY KEY (rec_id);


--
-- TOC entry 5787 (class 2606 OID 20484)
-- Name: movimentos movimentos_pkey; Type: CONSTRAINT; Schema: cadastro; Owner: postgres
--

ALTER TABLE ONLY cadastro.movimentos
    ADD CONSTRAINT movimentos_pkey PRIMARY KEY (rec_id);


--
-- TOC entry 5785 (class 2606 OID 20477)
-- Name: residentes residentes_pkey; Type: CONSTRAINT; Schema: cadastro; Owner: postgres
--

ALTER TABLE ONLY cadastro.residentes
    ADD CONSTRAINT residentes_pkey PRIMARY KEY (rec_id);


--
-- TOC entry 5775 (class 2606 OID 20441)
-- Name: talhao talhao_pkey; Type: CONSTRAINT; Schema: cadastro; Owner: postgres
--

ALTER TABLE ONLY cadastro.talhao
    ADD CONSTRAINT talhao_pkey PRIMARY KEY (rec_id);


--
-- TOC entry 5777 (class 2606 OID 20449)
-- Name: tipos_construcao tipos_construcao_pkey; Type: CONSTRAINT; Schema: cadastro; Owner: postgres
--

ALTER TABLE ONLY cadastro.tipos_construcao
    ADD CONSTRAINT tipos_construcao_pkey PRIMARY KEY (rec_id);


--
-- TOC entry 5783 (class 2606 OID 20470)
-- Name: tipos_movimentos tipos_movimentos_pkey; Type: CONSTRAINT; Schema: cadastro; Owner: postgres
--

ALTER TABLE ONLY cadastro.tipos_movimentos
    ADD CONSTRAINT tipos_movimentos_pkey PRIMARY KEY (rec_id);


--
-- TOC entry 5791 (class 2606 OID 20509)
-- Name: cartografia cartografia_pkey; Type: CONSTRAINT; Schema: inf_geografica; Owner: postgres
--

ALTER TABLE ONLY inf_geografica.cartografia
    ADD CONSTRAINT cartografia_pkey PRIMARY KEY (rec_id);


--
-- TOC entry 5793 (class 2606 OID 20516)
-- Name: cartografialayers cartografialayers_pkey; Type: CONSTRAINT; Schema: inf_geografica; Owner: postgres
--

ALTER TABLE ONLY inf_geografica.cartografialayers
    ADD CONSTRAINT cartografialayers_pkey PRIMARY KEY (rec_id);


--
-- TOC entry 5768 (class 1259 OID 20425)
-- Name: construcoes_gis; Type: INDEX; Schema: cadastro; Owner: postgres
--

CREATE INDEX construcoes_gis ON cadastro.construcoes USING gist (geometria);


--
-- TOC entry 5773 (class 1259 OID 20442)
-- Name: talhao_gis; Type: INDEX; Schema: cadastro; Owner: postgres
--

CREATE INDEX talhao_gis ON cadastro.talhao USING gist (geometria);


-- Completed on 2025-04-28 09:51:52

--
-- PostgreSQL database dump complete
--

