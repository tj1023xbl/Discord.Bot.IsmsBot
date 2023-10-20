import { Chip, CircularProgress, createTheme, Stack } from '@mui/material'
import { ThemeProvider } from '@emotion/react';
import styleVariables from '../variables.module.scss'
import { getAllGuildsAsyncThunk, Guild, setActiveGuild } from '../data/store/GuildSlice';
import { useSelector } from 'react-redux';
import { AppDispatch, RootState, Status } from '../data/store/Store';
import { useEffect } from 'react';

export const PillSection = () => {
    const guildsStatus: Status = useSelector((state: RootState) => state.Guilds.status);
    const guilds = useSelector((state: RootState) => state.Guilds.guilds);
    const activeGuild = useSelector((state: RootState) => state.Guilds.activeGuild);

    const handleClick = (guild: Guild) => {
        AppDispatch(setActiveGuild(guild))
    }

    useEffect(() => {
        // Grab all the guilds
        AppDispatch(getAllGuildsAsyncThunk());

    }, [])

    const customTheme = createTheme({
        components: {
            MuiChip: {
                styleOverrides: {
                    root: {
                        color: 'whitesmoke'
                    }
                },
                variants: [
                    {
                        props: { variant: 'filled' },
                        style: {
                            backgroundColor: styleVariables.yellow_light,
                            color: styleVariables.background_dark,
                            "&:hover": {
                                backgroundColor: `${styleVariables.yellow_dark} !important`
                            },
                        }
                    },
                    {
                        props: { variant: 'outlined' },
                        style: {
                            backgroundColor: styleVariables.background_light,
                            "&:hover": {
                                backgroundColor: `${styleVariables.background_dark} !important`
                            },

                        }
                    },
                ]
            }
        }
    })

    return (
        <>
            {
                guildsStatus === 'loading' ?
                    (
                        <CircularProgress />
                    )
                    :
                    (
                        <Stack direction="row" spacing={1}>
                            <ThemeProvider theme={customTheme}>
                                {guilds.map((guild) => {
                                    return <Chip onClick={() => handleClick(guild)} label={guild.name} key={guild.id} variant={guild.id === activeGuild?.id ? 'filled' : 'outlined'} ></Chip>
                                })}
                            </ThemeProvider>
                        </Stack>
                    )
            }
        </>
    )
}
