--
-- PostgreSQL database dump
--

-- Dumped from database version 17.4
-- Dumped by pg_dump version 17.4

-- Started on 2025-09-07 20:48:20

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

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 229 (class 1259 OID 19305)
-- Name: funcionalidades; Type: TABLE; Schema: acesso; Owner: postgres
--

CREATE TABLE acesso.funcionalidades (
    codigo_funcionalidade integer NOT NULL,
    designacao character varying(100) NOT NULL,
    codigo_pai integer
);


ALTER TABLE acesso.funcionalidades OWNER TO postgres;

--
-- TOC entry 224 (class 1259 OID 19283)
-- Name: grupos; Type: TABLE; Schema: acesso; Owner: postgres
--

CREATE TABLE acesso.grupos (
    rec_id integer NOT NULL,
    nome character varying(100) NOT NULL,
    activo boolean DEFAULT true
);


ALTER TABLE acesso.grupos OWNER TO postgres;

--
-- TOC entry 223 (class 1259 OID 19282)
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
-- TOC entry 5943 (class 0 OID 0)
-- Dependencies: 223
-- Name: grupos_rec_id_seq; Type: SEQUENCE OWNED BY; Schema: acesso; Owner: postgres
--

ALTER SEQUENCE acesso.grupos_rec_id_seq OWNED BY acesso.grupos.rec_id;


--
-- TOC entry 228 (class 1259 OID 19299)
-- Name: grupos_utilizadores; Type: TABLE; Schema: acesso; Owner: postgres
--

CREATE TABLE acesso.grupos_utilizadores (
    rec_id integer NOT NULL,
    utilizador_id integer NOT NULL,
    grupo_id integer NOT NULL
);


ALTER TABLE acesso.grupos_utilizadores OWNER TO postgres;

--
-- TOC entry 227 (class 1259 OID 19298)
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
-- TOC entry 5944 (class 0 OID 0)
-- Dependencies: 227
-- Name: grupos_utilizadores_rec_id_seq; Type: SEQUENCE OWNED BY; Schema: acesso; Owner: postgres
--

ALTER SEQUENCE acesso.grupos_utilizadores_rec_id_seq OWNED BY acesso.grupos_utilizadores.rec_id;


--
-- TOC entry 231 (class 1259 OID 19311)
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
-- TOC entry 230 (class 1259 OID 19310)
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
-- TOC entry 5945 (class 0 OID 0)
-- Dependencies: 230
-- Name: permissoes_rec_id_seq; Type: SEQUENCE OWNED BY; Schema: acesso; Owner: postgres
--

ALTER SEQUENCE acesso.permissoes_rec_id_seq OWNED BY acesso.permissoes.rec_id;


--
-- TOC entry 226 (class 1259 OID 19291)
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
-- TOC entry 225 (class 1259 OID 19290)
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
-- TOC entry 5946 (class 0 OID 0)
-- Dependencies: 225
-- Name: utilizadores_rec_id_seq; Type: SEQUENCE OWNED BY; Schema: acesso; Owner: postgres
--

ALTER SEQUENCE acesso.utilizadores_rec_id_seq OWNED BY acesso.utilizadores.rec_id;


--
-- TOC entry 5761 (class 2604 OID 19286)
-- Name: grupos rec_id; Type: DEFAULT; Schema: acesso; Owner: postgres
--

ALTER TABLE ONLY acesso.grupos ALTER COLUMN rec_id SET DEFAULT nextval('acesso.grupos_rec_id_seq'::regclass);


--
-- TOC entry 5765 (class 2604 OID 19302)
-- Name: grupos_utilizadores rec_id; Type: DEFAULT; Schema: acesso; Owner: postgres
--

ALTER TABLE ONLY acesso.grupos_utilizadores ALTER COLUMN rec_id SET DEFAULT nextval('acesso.grupos_utilizadores_rec_id_seq'::regclass);


--
-- TOC entry 5766 (class 2604 OID 19314)
-- Name: permissoes rec_id; Type: DEFAULT; Schema: acesso; Owner: postgres
--

ALTER TABLE ONLY acesso.permissoes ALTER COLUMN rec_id SET DEFAULT nextval('acesso.permissoes_rec_id_seq'::regclass);


--
-- TOC entry 5763 (class 2604 OID 19294)
-- Name: utilizadores rec_id; Type: DEFAULT; Schema: acesso; Owner: postgres
--

ALTER TABLE ONLY acesso.utilizadores ALTER COLUMN rec_id SET DEFAULT nextval('acesso.utilizadores_rec_id_seq'::regclass);


--
-- TOC entry 5935 (class 0 OID 19305)
-- Dependencies: 229
-- Data for Name: funcionalidades; Type: TABLE DATA; Schema: acesso; Owner: postgres
--

INSERT INTO acesso.funcionalidades VALUES (500, 'Gestão de Utilizadores', NULL);
INSERT INTO acesso.funcionalidades VALUES (1000, 'Árvore de Cartografia', NULL);


--
-- TOC entry 5930 (class 0 OID 19283)
-- Dependencies: 224
-- Data for Name: grupos; Type: TABLE DATA; Schema: acesso; Owner: postgres
--



--
-- TOC entry 5934 (class 0 OID 19299)
-- Dependencies: 228
-- Data for Name: grupos_utilizadores; Type: TABLE DATA; Schema: acesso; Owner: postgres
--



--
-- TOC entry 5937 (class 0 OID 19311)
-- Dependencies: 231
-- Data for Name: permissoes; Type: TABLE DATA; Schema: acesso; Owner: postgres
--



--
-- TOC entry 5932 (class 0 OID 19291)
-- Dependencies: 226
-- Data for Name: utilizadores; Type: TABLE DATA; Schema: acesso; Owner: postgres
--

INSERT INTO acesso.utilizadores VALUES (4, 'admin', 'admin', '$2b$12$oZ2lSMDJhP4uJlSW8UOA5.gMbIoajXdk.073K5GcKU.3tu83vLpz.', '', true);


--
-- TOC entry 5947 (class 0 OID 0)
-- Dependencies: 223
-- Name: grupos_rec_id_seq; Type: SEQUENCE SET; Schema: acesso; Owner: postgres
--

SELECT pg_catalog.setval('acesso.grupos_rec_id_seq', 1, false);


--
-- TOC entry 5948 (class 0 OID 0)
-- Dependencies: 227
-- Name: grupos_utilizadores_rec_id_seq; Type: SEQUENCE SET; Schema: acesso; Owner: postgres
--

SELECT pg_catalog.setval('acesso.grupos_utilizadores_rec_id_seq', 1, false);


--
-- TOC entry 5949 (class 0 OID 0)
-- Dependencies: 230
-- Name: permissoes_rec_id_seq; Type: SEQUENCE SET; Schema: acesso; Owner: postgres
--

SELECT pg_catalog.setval('acesso.permissoes_rec_id_seq', 1, false);


--
-- TOC entry 5950 (class 0 OID 0)
-- Dependencies: 225
-- Name: utilizadores_rec_id_seq; Type: SEQUENCE SET; Schema: acesso; Owner: postgres
--

SELECT pg_catalog.setval('acesso.utilizadores_rec_id_seq', 4, true);


--
-- TOC entry 5775 (class 2606 OID 19309)
-- Name: funcionalidades funcionalidades_pkey; Type: CONSTRAINT; Schema: acesso; Owner: postgres
--

ALTER TABLE ONLY acesso.funcionalidades
    ADD CONSTRAINT funcionalidades_pkey PRIMARY KEY (codigo_funcionalidade);


--
-- TOC entry 5769 (class 2606 OID 19289)
-- Name: grupos grupos_pkey; Type: CONSTRAINT; Schema: acesso; Owner: postgres
--

ALTER TABLE ONLY acesso.grupos
    ADD CONSTRAINT grupos_pkey PRIMARY KEY (rec_id);


--
-- TOC entry 5773 (class 2606 OID 19304)
-- Name: grupos_utilizadores grupos_utilizadores_pkey; Type: CONSTRAINT; Schema: acesso; Owner: postgres
--

ALTER TABLE ONLY acesso.grupos_utilizadores
    ADD CONSTRAINT grupos_utilizadores_pkey PRIMARY KEY (rec_id);


--
-- TOC entry 5777 (class 2606 OID 19317)
-- Name: permissoes permissoes_pkey; Type: CONSTRAINT; Schema: acesso; Owner: postgres
--

ALTER TABLE ONLY acesso.permissoes
    ADD CONSTRAINT permissoes_pkey PRIMARY KEY (rec_id);


--
-- TOC entry 5771 (class 2606 OID 19297)
-- Name: utilizadores utilizadores_pkey; Type: CONSTRAINT; Schema: acesso; Owner: postgres
--

ALTER TABLE ONLY acesso.utilizadores
    ADD CONSTRAINT utilizadores_pkey PRIMARY KEY (rec_id);


-- Completed on 2025-09-07 20:48:20

--
-- PostgreSQL database dump complete
--

