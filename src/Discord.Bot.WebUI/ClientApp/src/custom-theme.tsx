import { colors, createTheme, iconButtonClasses, inputBaseClasses, outlinedInputClasses, typographyClasses } from "@mui/material";
import styleVariables from './variables.module.scss'
import { dark } from "@mui/material/styles/createPalette";

const customTheme = createTheme({
    palette: {
        mode: 'dark',
        primary: colors.yellow,
        background: {
            default: '#000',
            paper: `${styleVariables.background_dark}`
        },
        text: {
            primary: '#fff'
        }
    },
    components: {
        MuiContainer: {
            defaultProps: {
                maxWidth: false
            },
            styleOverrides: {
                root: {
                        backgroundColor: `${styleVariables.background_dark}`,
                        display: 'flex',
                        flexDirection: 'column',
                        flex: '1',
                        padding: '0 20px',
                        color: 'whitesmoke',
                    
                        [`& section:first-of-type`]: {
                            display: 'flex',
                            justifyContent: 'space-between',
                            alignItems: 'center'
                            
                        }
                    }                
            }
        },
        MuiTypography: {
            styleOverrides: {
                root: {
                    [`&.${typographyClasses.root}`]: {
                        color: 'whitesmoke'
                    }
                },
            }
        },
       
        MuiFormLabel: {
            styleOverrides: {
                root: {
                    color: 'whitesmoke !important'
                }
            }
        },
        MuiOutlinedInput: {
            styleOverrides: {
                root: {
                    borderColor: 'white !important',
                    color: 'white',
                    [`.${outlinedInputClasses.focused}`]: {
                        borderColor: `${styleVariables.yellow_dark}  !important`,
                        color: `${styleVariables.yellow_dark}  !important`,
                    },
                },
                notchedOutline: {
                    borderColor: 'white',

                },
            }
        },
        MuiInputBase: {
            styleOverrides: {
                root: {
                    marginBottom: '15px',
                    [`&${outlinedInputClasses.focused}.${outlinedInputClasses.notchedOutline}.${outlinedInputClasses.root}.MuiInputBase-colorPrimary`]: {
                        borderColor: styleVariables.yellow_light,
                        color: `${styleVariables.yellow_dark}  !important`,
                    },
                }
            }
        },
    }
})
export default customTheme;