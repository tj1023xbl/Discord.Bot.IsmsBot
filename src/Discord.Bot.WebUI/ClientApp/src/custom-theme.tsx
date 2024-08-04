import { colors, createTheme, iconButtonClasses, inputBaseClasses, outlinedInputClasses, typographyClasses } from "@mui/material";
import styleVariables from './variables.module.scss'

const customTheme = createTheme({
    palette: {
        primary: colors.yellow,
        background: {
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
        MuiTableContainer: {
            styleOverrides: {
                root: {
                    backgroundColor: 'unset'
                }
            }
        },
        MuiTableRow: {
            styleOverrides: {
                head: {
                    backgroundColor: `#101010 !important`,
                    borderRadius: 'unset',
                    ['&:hover']: {
                        backgroundColor: `#101010 !important`,
                    }

                },
                root: {
                    backgroundColor: '#404040',
                    ['&:nth-of-type(odd)']: {
                        backgroundColor: '#303030',
                    },
                    ['&:hover']: {
                        backgroundColor: '#202020 !important',
                    }
                },
            },
        },
        MuiTableCell: {
            styleOverrides: {
                root: {
                    color: `whitesmoke`,
                    border: 'unset !important',
                },
                head: {
                    color: `${styleVariables.yellow_dark} !important`,
                }
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