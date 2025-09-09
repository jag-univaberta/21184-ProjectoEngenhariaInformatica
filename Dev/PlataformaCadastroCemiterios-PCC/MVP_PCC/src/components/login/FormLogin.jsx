//
import * as React from 'react';
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import CssBaseline from '@mui/material/CssBaseline';
import TextField from '@mui/material/TextField';
import FormControlLabel from '@mui/material/FormControlLabel';
import Checkbox from '@mui/material/Checkbox';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import { createTheme, ThemeProvider, alpha, getContrastRatio } from '@mui/material/styles'; 
 
import * as Actions from "../../constants/actions";
import { connect } from "react-redux";
import { processData, decodeHtmlEntities } from '../../utils/utils';

function Copyright(props) {
  return (
    <div style={{ position: 'fixed',
      bottom: 0,
      left: 0,
      width: '100%',
      display: 'flex',
      justifyContent: 'center',
      alignItems: 'center'
     }}>
    <Typography variant="body2" color="text.secondary" align="center" {...props}>
      {'Copyright © '}   {new Date().getFullYear()} {' - '}
        José Augusto Azevedo - Nº 2200655 - 
       <br>
        </br>Licenciatura em Engenharia Informática
    </Typography>
    </div>
  );
}

class FormLogin extends React.Component {

  constructor(props) {
    super(props);
    this.formRef = React.createRef();
    this.handleDashboardSessionId = this.handleDashboardSessionId.bind(this);
    this.state = {
      errorMessage: '',

    }
  }

  handleSubmit = async (event) => {
    event.preventDefault();
    document.body.style.cursor = 'wait';
    const data = new FormData(event.currentTarget);
    console.log({
      username: data.get('username'),
      password: data.get('password'),
    });
    let now = new Date();
    let year = now.getFullYear().toString();
    let month = ('0' + (now.getMonth() + 1)).slice(-2);
    let day = ('0' + now.getDate()).slice(-2);
    let hours = ('0' + now.getHours()).slice(-2);
    let minutes = ('0' + now.getMinutes()).slice(-2);
    let seconds = ('0' + now.getSeconds()).slice(-2);    
    let yyyymmddHHmmss = year + month + day + hours + minutes + seconds;
    let tkid=this.props.config.aplicationTokenId;
    
    try {
      const apiEndpoint = this.props.config.configapiEndpoint;
		  const url = apiEndpoint + 'Auth/login';
      const response = await fetch(url, {
        method: 'POST',
        body: JSON.stringify({
          username: data.get('username'),
          password: data.get('password'), 
        }),
        headers: {
          'Content-Type': 'application/json',
        },
      });
      document.body.style.cursor = 'default';
      if (response.ok) {
        const res = await response.json();
        console.log(res);

        const token = res.authtoken;
        const applicationname = res.applicationname;
        const applicationsigla = res.applicationsigla;
        const aux_userid = res.userid;
        const aux_username = res.username;
        const aux_usersession= res.usersession;
        const aux_permissoes = res.permissoes;
        const aux_separadores_app = res.separadores_app;
        const aux_separadores_escrita = res.separadores_escrita;
        const aux_separadores_leitura = res.separadores_leitura;
        const aux_sep_nomes = res.separadores.split("|");
        const aux_sep_tooltips = res.separadorestooltips.split("|");        
        const aux_sep_ids = res.separadoresids.split("|");
        const aux_sep_sigla = res.separadoressiglas.split("|");
        const aux_sep_data = res.separadoresdata.split("{#}");
        const aux_treeconstrucoes_data = processData(res.treeconstrucoes_data);
        const aux_treecartografia_data = processData(res.treecartografia_data);
        const aux_treecartografia_init = res.treecartografia_init.split(";");
        const aux_treeseparador_data = decodeHtmlEntities(res.treeseparador_data);
        const aux_mapa_defs = res.mapa_defs.split(";");
        const aux_layers_iniciais = res.layers_iniciais.split("|");
        const aux_mapa_initialview_x = aux_mapa_defs[0];
        const aux_mapa_initialview_y = aux_mapa_defs[1];
        const aux_mapa_initialview_escala = aux_mapa_defs[4];   
        const aux_final = [];        
        for (let i = 0; i < aux_sep_data.length; i++) {
          var a= aux_sep_data[i];//.split("#");
          aux_final.push(a);
        }
        if (aux_sep_nomes[aux_sep_nomes.length - 1] == '') { 
          aux_sep_nomes.pop(); 
          aux_sep_ids.pop(); 
          aux_sep_sigla.pop(); 
        }        
        if (aux_userid!=''){      
          this.props.dispatchLogin(
            {payload: 
              { authtoken: token, 
                aplicacao_titulo:applicationname,  
                aplicacao_sigla:applicationsigla, 
                userid: aux_userid, 
                username: aux_username, 
                usersession: aux_usersession, 
                permissoes: aux_permissoes, 
                sep_app: aux_separadores_app, 
                sep_write: aux_separadores_escrita, 
                sep_read: aux_separadores_leitura, 
                separadoresnomes: aux_sep_nomes, 
                separadorestooltips: aux_sep_tooltips,
                separadoresids: aux_sep_ids, 
                separadoressiglas: aux_sep_sigla, 
                separadoresdata: aux_final,
                treeconstrucoes_data: aux_treeconstrucoes_data,
                treeseparador_data: aux_treeseparador_data, 
                treecartografia_data: aux_treecartografia_data,
                treecartografia_init: aux_treecartografia_init,
                layers_iniciais: aux_layers_iniciais, 
                mapa_initialview_x: aux_mapa_initialview_x,
                mapa_initialview_y: aux_mapa_initialview_y, 
                mapa_initialview_escala: aux_mapa_initialview_escala,
              } 
            }
          );
        } else {
          this.setState({ errorMessage: 'Utilizador não encontrado ou palavra chave errada.' });
        }
      } else {
        console.error('Error:', response.status, response.statusText);
        this.setState({ errorMessage: 'Acesso não autorizado.' });
         // Clear the error message after 5 seconds (5000 milliseconds)
        setTimeout(() => {
          this.setState({ errorMessage: '' });
        }, 5000);
      }      
    } catch (error) {
      console.error('error: ' + error);
      this.setState({ errorMessage: 'Ocorreu um erro ao efetuar login.' });
       // Clear the error message after 5 seconds (5000 milliseconds)
       setTimeout(() => {
        this.setState({ errorMessage: '' });
      }, 5000);
    }
  }


  handleDashboardSessionId = async () => {
     
    let now = new Date();
    let year = now.getFullYear().toString();
    let month = ('0' + (now.getMonth() + 1)).slice(-2);
    let day = ('0' + now.getDate()).slice(-2);
    let hours = ('0' + now.getHours()).slice(-2);
    let minutes = ('0' + now.getMinutes()).slice(-2);
    let seconds = ('0' + now.getSeconds()).slice(-2);
    
    let yyyymmddHHmmss = year + month + day + hours + minutes + seconds;
    document.body.style.cursor = 'wait';
    let ssid=this.props.config.startSessionId;
    let tkid=this.props.config.aplicationTokenId;
    // Aqui é entrada com base na sessionid para a app com o tokenid
    try {
      const apiEndpoint = this.props.config.configapiEndpoint;
		  const url = apiEndpoint + 'authentication';
      const response = await fetch(url, {
        method: 'POST',
        body: JSON.stringify({
          sessionid: ssid, 
          tokenappid: tkid,
          datahora: yyyymmddHHmmss,
        }),
        headers: {
          'Content-Type': 'application/json',
        },
      });
      document.body.style.cursor = 'default';
      if (response.ok) {
        const res = await response.json();
        console.log(res); 
        const token = res.authtoken;
        const applicationname = res.applicationname;
        const applicationsigla = res.applicationsigla;

        const aux_userid = res.userid;
        const aux_username = res.username;
        const aux_usersession= res.usersession;
        const aux_permissoes = res.permissoes;

        const aux_separadores_app = res.separadores_app;
        const aux_separadores_escrita = res.separadores_escrita;
        const aux_separadores_leitura = res.separadores_leitura;

        const aux_sep_nomes = res.separadores.split("|");
        const aux_sep_tooltips = res.separadorestooltips.split("|");
        
        const aux_sep_ids = res.separadoresids.split("|");
        const aux_sep_sigla = res.separadoressiglas.split("|");
        const aux_sep_data = res.separadoresdata.split("{#}");

        
        const aux_treecartografia_init = res.treecartografia_init.split(";");
        const aux_treeinstrumentos_init = res.treeinstrumentos_init.split(";");
        const aux_layers_iniciais = res.layers_iniciais.split("|");
        const aux_mapa_defs = res.mapa_defs.split(";");
        
        const aux_mapa_initialview_x = aux_mapa_defs[0];
        const aux_mapa_initialview_y = aux_mapa_defs[1];
        const aux_mapa_initialview_escala = aux_mapa_defs[4];
  
        const aux_treepois_data = processData(res.treepois_data);
        const aux_treeconstrucoes_data = processData(res.treeconstrucoes_data);
        const aux_treecartografia_data = processData(res.treecartografia_data);
        const aux_treeinstrumentos_data = processData(res.treeinstrumentos_data);
        const aux_treeatendimento_data = processData(res.treeatendimento_data);
        const aux_treeseparador_data = res.treeseparador_data;
        
        //const aux_treecadastro_data = res.treecadastro_data;

        const aux_final = [];
        
        for (let i = 0; i < aux_sep_data.length; i++) {
          var a= aux_sep_data[i];//.split("#");
          aux_final.push(a);
        }

        if (aux_sep_nomes[aux_sep_nomes.length - 1] == '') { 
          aux_sep_nomes.pop(); 
          aux_sep_ids.pop(); 
          aux_sep_sigla.pop(); 
        }
 
        if (aux_userid!=''){
          
          this.props.dispatchLogin(
            {
              payload: 
              {
                authtoken: token, 
                aplicacao_titulo:applicationname,  
                aplicacao_sigla:applicationsigla,  
                userid: aux_userid, 
                username: aux_username, 
                usersession: aux_usersession,  
                permissoes: aux_permissoes, 
                sep_app: aux_separadores_app, 
                sep_write: aux_separadores_escrita, 
                sep_read: aux_separadores_leitura, 
                separadoresnomes: aux_sep_nomes, 
                separadorestooltips: aux_sep_tooltips,
                separadoresids: aux_sep_ids, 
                separadoressiglas: aux_sep_sigla, 
                separadoresdata: aux_final, 
                treepois_data: aux_treepois_data,
                treeconstrucoes_data: aux_treeconstrucoes_data,
                treeinstrumentos_data: aux_treeinstrumentos_data, 
                treeatendimento_data: aux_treeatendimento_data, 
                treeseparador_data: aux_treeseparador_data, 
                treecartografia_data: aux_treecartografia_data,
                treecartografia_init: aux_treecartografia_init,
                treeinstrumentos_init: aux_treeinstrumentos_init,
                //treecadastro_data: aux_treecadastro_data,
                layers_iniciais: aux_layers_iniciais, 
                mapa_initialview_x: aux_mapa_initialview_x,
                mapa_initialview_y: aux_mapa_initialview_y, 
                mapa_initialview_escala: aux_mapa_initialview_escala,

              } 
            });
        } else {
          this.setState({ errorMessage: 'Utilizador não encontrado ou palavra chave errada.' });
        }
      } else {
        const errorMessage = await response.text();
        console.log(errorMessage);
        this.setState({ errorMessage: errorMessage });
      }
    } catch (error) {
      console.error('error: ' + error);
      console.log(error);
      this.setState({ errorMessage: 'Ocorreu um erro ao efetuar login.' });
    }
  }
  componentDidUpdate() { 
    if ((this.props.config.configapiEndpoint!=='') && (this.props.config.startSessionId!=='')){
      //this.handleDashboardSessionId(); // Começar com um SessionId que veio do DashBoard
    }
  }
  render() {
    let processo=false;
   /* if ((this.props.config.configapiEndpoint!=='') && (this.props.config.startSessionId!=='')){
      processo=true;
    }else {      
      processo=false;
    }*/
    const violetBase = '#230047';
    const violetMain = alpha(violetBase, 0.7);
    const theme = createTheme({
      palette: {
        violet: {
          main: violetMain,
          light: alpha(violetBase, 0.5),
          dark: alpha(violetBase, 0.9),
          contrastText: getContrastRatio(violetMain, '#fff') > 4.5 ? '#fff' : '#111',
        },
      },
    });
    const defaultUsername = "admin"; // Set your default username here
    const defaultPassword = "tooruab"; // Set your default password here 
    return processo ? (
      <ThemeProvider theme={theme}>
        <Container component="main" maxWidth="xs">
          <CssBaseline />
          <Box
            sx={{
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'center',
            }}
          > 
            <Typography component="h1" variant="h5">
              A iniciar Sessão ...
            </Typography> 
          </Box>
          <Copyright sx={{ mt: 8, mb: 4 }} />
        </Container> 
      </ThemeProvider>
    ):  
      (
      <ThemeProvider theme={theme}>
        <Container component="main" maxWidth="xs">
          <CssBaseline />
          <Box
            sx={{
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'center',
            }}
          > 
            <Typography component="h1" variant="h5">
              Iniciar Sessão
            </Typography>
            <Box component="form" onSubmit={this.handleSubmit} noValidate sx={{ mt: 1 }}>
              <TextField
                margin="normal"
                required
                fullWidth
                id="username"
                label="Utilizador"
                name="username"
                autoComplete="username"
                autoFocus 
                defaultValue={defaultUsername}
              />
              <TextField
                margin="normal"
                required
                fullWidth
                name="password"
                label="Palavra-passe"
                type="password"
                id="password"
                autoComplete="current-password"
                defaultValue={defaultPassword}
              />
              <FormControlLabel
                control={<Checkbox value="remember" color="primary" />}
                label="Lembrar-me"
              />
              <Button
                type="submit"
                fullWidth
                variant="contained"
                //color="violet.main"
                //sx={{ mt: 3, mb: 2 }}
                sx={{
                  mt: 3,
                  mb: 2,
                  backgroundColor: 'violet.main', // Acede diretamente à cor da paleta
                  color: 'violet.contrastText',   // Para o texto, para garantir bom contraste
                  '&:hover': {
                    backgroundColor: 'violet.dark', // Opcional: cor para o hover
                  },
                }}
              >
                Entrar
              </Button>
              <Grid container>
                <Grid item xs>
                  <Link href="#" variant="body2">
                    Recuperar palavra-passe?
                  </Link>
                </Grid> 
              </Grid>
              <Typography align="center" color="error">
                {this.state.errorMessage  ? this.state.errorMessage : ""}         
              </Typography> 
              {/*(() => { 
                if (this.state.errorMessage !="") {
                  <Typography variant="body" color="error">
                    {this.state.errorMessage}
                  </Typography> 
                }
              })()*/}
            </Box>
          </Box>
          <Copyright sx={{ mt: 8, mb: 4 }} />
        </Container> 
      </ThemeProvider>
    );
  }
}

//Mapear o estado para as propriedades do objeto
function mapStateToProps(state){
	return {
		arv : state.message,  
    config: state.aplicacaopcc.config,
	};
}
//Envia nova propriedades para o estado
function mapDispatchToProps(dispatch){

	return {
		dispatchLogin: (item) => dispatch({
			type: Actions.LOGIN, payload: item.payload
		}),
	};
}
 
export default connect(mapStateToProps,mapDispatchToProps)(FormLogin);

 
