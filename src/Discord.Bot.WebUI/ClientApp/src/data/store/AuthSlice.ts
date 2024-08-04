import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import axios, { HttpStatusCode } from "axios";
import { AppDispatch, Status } from "./Store";

export const GetAuthenticationStatusAsyncThunk = createAsyncThunk(
    'thunks/GetAuthenticationStatusAsyncThunk',
    async (props) => {
        const url = '/api/account/info';
        const authResponse = await axios<string>({
            method: 'get',
            url: url,
            responseType: 'json',
            headers: {
                'Content-Type': 'application-json'
            }
        });
        if (authResponse.status === HttpStatusCode.Ok){
            return AuthStatus.authorized;
        }
        else {
            return AuthStatus.unauthorized;
        }
    }

)

export interface AuthState {
    authStatus: AuthStatus;
    error: string | null;
    requestStatus: Status
}

export enum AuthStatus {
    authorized = 'AUTHORIZED',
    unauthorized = 'UNAUTHORIZED'
}


export const AuthSlice = createSlice({
    name: 'AuthSlice',
    initialState: {
        authStatus: AuthStatus.unauthorized,
        error: null
    } as AuthState,
    reducers: {
        
    },
    extraReducers: builder => {
        builder.addCase(GetAuthenticationStatusAsyncThunk.pending, (state) => {
            state.error = null,
            state.authStatus = AuthStatus.unauthorized,
            state.requestStatus = 'loading'
        });

        builder.addCase(GetAuthenticationStatusAsyncThunk.fulfilled, (state, action: any) => {
            state.error = null,
            state.authStatus = action.payload as AuthStatus
            state.requestStatus = 'complete'

        });

        builder.addCase(GetAuthenticationStatusAsyncThunk.rejected, (state, action: any) => {
            state.error = action.error.message
            state.authStatus = AuthStatus.unauthorized
            state.requestStatus = 'failed'
        });
    }

})