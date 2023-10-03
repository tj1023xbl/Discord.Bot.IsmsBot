import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import axios from 'axios';


export interface Guild {
    id: number,
    name: string;
}

export interface IsmState {
    status: 'loading' | 'idle' | 'complete' | 'failed';
    error: string | null;
    value: Guild[];
}

/**
 * Thunk used to pull all guilds from the db via API
 */
export const getAllGuildsAsyncThunk = createAsyncThunk(
    'thunks/createAsyncThunk',
    async (props) => {
        const url = '/api/Isms/GetAllGuildsAsync';
        const getAllGuildsResponse = await axios({
            method: 'get',
            url: url,
            responseType: 'json',
            headers: {
                'Content-Type': 'application-json'
            }
        });
        return getAllGuildsResponse.data;
    }
)

export const IsmSlice = createSlice({
    name: 'IsmSlice',
    initialState: {
        status: 'idle',
        value: [],
        error: null
    } as IsmState,
    reducers: {},
    extraReducers: (builder) => {
        builder.addCase(getAllGuildsAsyncThunk.pending, (state) => {
            state.status = 'loading';
            state.error = null;
            state.value = [];
        });
        builder.addCase(getAllGuildsAsyncThunk.fulfilled, (state, action: any) => {
            state.error = null;
            state.status = 'complete';
            state.value = action.payload;
        });
        builder.addCase(getAllGuildsAsyncThunk.rejected, (state, action: any) => {
            state.error = action.error.message;
            state.status = 'failed';
            state.value = [];
        });

    }
})