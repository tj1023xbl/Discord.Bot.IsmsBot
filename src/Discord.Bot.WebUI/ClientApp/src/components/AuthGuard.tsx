import { useSelector } from 'react-redux';
import { AuthSlice, AuthStatus, GetAuthenticationStatusAsyncThunk } from '../data/store/AuthSlice';
import { AppDispatch, RootState, Status } from '../data/store/Store';
import { Login } from './Login';
import { useEffect } from 'react';
import { BottomNavigation, BottomNavigationAction, CircularProgress, Container, Typography } from '@mui/material';
import { NavigationOutlined } from '@mui/icons-material';


const loading = (status: Status, authStatus: AuthStatus, children: any) => {
    if (status === 'loading') {
        return (
            <Container>
                <center>
                    <CircularProgress size={100} />
                </center>
            </Container>
        )
    }
    else if (authStatus === AuthStatus.unauthorized) {
        return (
            <Container>
                <Login />
            </Container>
        )
    }
    else {
        return children
    }
}

export const AuthGuard = ({ children }: { children: any }) => {

    const authStatus = useSelector((state: RootState) => state.AuthState.authStatus);
    const requestStatus = useSelector((state: RootState) => state.AuthState.requestStatus);

    useEffect(() => {

        AppDispatch(GetAuthenticationStatusAsyncThunk());

    }, [])

    return loading(requestStatus, authStatus, children)

}