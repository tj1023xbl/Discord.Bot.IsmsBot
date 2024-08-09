import { ThemeProvider } from '@emotion/react';
import { useEffect } from 'react';
import { useSelector } from 'react-redux';
import { IsmTable } from './components/IsmTable';
import { PillSection } from './components/PillSection';
import { getAllSayingsAsyncThunk } from './data/store/IsmSlice';
import { AppDispatch, RootState } from './data/store/Store';
import { AuthStatus, setAuthStatus } from './data/store/AuthSlice';
import { AuthGuard } from './components/AuthGuard';
import { Avatar, Button, Container } from '@mui/material';
import customTheme from './custom-theme';
import axios from 'axios';
import { Loading } from './components/Loading';

/**
 * Homepage
 * @returns
 */
export default function App() {
    const activeGuild = useSelector((state: RootState) => state.Guilds.activeGuild);
    const sayingsState = useSelector((state: RootState) => state.Isms);
    const authStatus = useSelector((state: RootState) => state.AuthState.authStatus);

    const logoMargin = 14;
    const logoHeight = 40;

    const signout = () => {
        axios('/api/account/logout', { method: 'get' });
        AppDispatch(setAuthStatus(AuthStatus.unauthorized));
    };

    useEffect(() => {
        if (activeGuild) {
            // Grab all the isms to display them in a table
            AppDispatch(getAllSayingsAsyncThunk(activeGuild.id))
        }
    }, [activeGuild?.id, authStatus])

    return (
        <ThemeProvider theme={customTheme}>
            <Loading />
                <AuthGuard>

                    <Container maxWidth={false}>
                        <section>
                            <Avatar sx={{ height: `${logoHeight}px`, width: `${logoHeight}px`, m: `${logoMargin}px ${logoMargin}px ${logoMargin}px 0px` }} src='/assets/maniacal.png' />
                            <PillSection />
                            <Button onClick={signout}>Sign Out</Button>
                        </section>
                        <section>
                            <IsmTable sayings={sayingsState.value} />
                        </section>
                    </Container>

                </AuthGuard>
    
        </ThemeProvider>
    )
}

