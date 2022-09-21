-- SEQUENCE: public.tb_user_user_id_seq

-- DROP SEQUENCE IF EXISTS public.tb_user_user_id_seq;

CREATE SEQUENCE IF NOT EXISTS public.tb_user_user_id_seq
    INCREMENT 1
    START 1
    MINVALUE 1
    MAXVALUE 9223372036854775807
    CACHE 1;

ALTER SEQUENCE public.tb_user_user_id_seq
    OWNER TO garage474_dev;


-- DROP TABLE IF EXISTS public.tb_user;

CREATE TABLE IF NOT EXISTS public.tb_user
(
    user_id integer NOT NULL DEFAULT nextval('tb_user_user_id_seq'::regclass),
    username character varying(50) COLLATE pg_catalog."default" NOT NULL,
    password character varying(100) COLLATE pg_catalog."default" NOT NULL,
    created_at timestamp without time zone NOT NULL,
    role character varying(10) COLLATE pg_catalog."default",
    CONSTRAINT tb_user_pkey PRIMARY KEY (user_id),
    CONSTRAINT tb_user_username_key UNIQUE (username)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.tb_user
    OWNER to postgres;

GRANT ALL ON TABLE public.tb_user TO garage474_dev;

GRANT ALL ON TABLE public.tb_user TO postgres;