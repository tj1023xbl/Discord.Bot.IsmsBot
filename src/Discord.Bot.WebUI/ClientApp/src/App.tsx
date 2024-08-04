import { ThemeProvider } from '@emotion/react';
import { useEffect } from 'react';
import { useSelector } from 'react-redux';
import styles from './App.module.scss';
import { IsmTable } from './components/IsmTable';
import { PillSection } from './components/PillSection';
import { getAllSayingsAsyncThunk } from './data/store/IsmSlice';
import { AppDispatch, RootState } from './data/store/Store';
import { AuthStatus } from './data/store/AuthSlice';
import { Routes, Route, Link, Navigate } from 'react-router-dom';
import { Login } from './components/Login';
import { Status } from './data/store/Store';

/**
 * Homepage
 * @returns
 */
export default function App() {
    const authState = useSelector((state: RootState) => state.AuthState);
    const activeGuild = useSelector((state: RootState) => state.Guilds.activeGuild);
    const sayingsState = useSelector((state: RootState) => state.Isms);

    useEffect(() => {
        if (activeGuild) {
            // Grab all the isms to display them in a table
            AppDispatch(getAllSayingsAsyncThunk(activeGuild.id))
        }
    }, [activeGuild?.id, authState.requestStatus, authState.authStatus])

    return (
        <>
            {
                authState.authStatus === AuthStatus.unauthorized ?
                    (
                    <div>
                            <h1>request status: {authState.requestStatus} </h1>
                            <h1>auth status: {authState.authStatus}</h1>
                            <Login />
                    </div>
                    )
                    :
                    (
                        <div className={styles.container}>
                            <h1>request status: {authState.requestStatus} </h1>
                            <h1>auth status: {authState.authStatus}</h1>
                            <section>
                                <h1 className={styles.intro}>Isms Bot</h1>
                                <PillSection />
                            </section>
                            <section>
                                <IsmTable sayings={sayingsState.value} loading={sayingsState.status === 'loading'} />
                            </section>
                        </div>
                    )
            }
        </>
    )
}

