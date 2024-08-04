import { useSelector } from 'react-redux';
import { AuthSlice, AuthStatus, GetAuthenticationStatusAsyncThunk } from '../data/store/AuthSlice';
import { AppDispatch, RootState, Status } from '../data/store/Store';
import { Login } from './Login';
import { useEffect } from 'react';
import { Container } from '@mui/material';

export const AuthGuard = ({ children }: { children: any }) => {

    const authStatus = useSelector((state: RootState) => state.AuthState.authStatus);
    const requestStatus = useSelector((state: RootState) => state.AuthState.requestStatus);
    var test;

    useEffect(() => {
        
        test = AppDispatch(GetAuthenticationStatusAsyncThunk());

    }, [])

    return <>
        {
            authStatus === AuthStatus.unauthorized ?
                (
                    <Container>
                        <h1>test: {test}</h1>
                        <h1>request status: {requestStatus} </h1>
                        <h1>auth status: {authStatus}</h1>
                        <Login />
                    </Container>
                )
                : (children)
        }
    </>
     
}