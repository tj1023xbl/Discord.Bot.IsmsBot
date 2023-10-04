import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import axios from 'axios';
import Saying from "../models/Saying";


export interface Guild {
    id: string,
    name: string;
}

export type Status = 'loading' | 'idle' | 'complete' | 'failed';

export interface IsmState {
    status: Status;
    error: string | null;
    value: Saying[];
}

/**
 * Thunk used to pull all guilds from the db via API
 */
export const getAllGuildsAsyncThunk = createAsyncThunk(
    'thunks/getAllGuildsAsyncThunk',
    async (props) => {
        const url = '/api/Isms/GetAllGuilds';
        const getAllGuildsResponse = await axios<Guild[]>({
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

export const getAllSayingsAsyncThunk = createAsyncThunk(
    'thunks/getAllSayingsAsyncThunk',
    async (guildId: string) => {
        const url = '/api/Isms/GetAllSayings/' + guildId
        const getAllSayingsResponse = await axios({
            method: 'get',
            url: url,
            responseType: 'json',
            headers: {
                'Content-Type': 'application-json'
            }
        });
        return getAllSayingsResponse.data
    }
)

export interface GuildState {
    status: Status;
    error: string | null;
    guilds: Guild[];
    activeGuild: Guild | null;
}

export const GuildSlice = createSlice({
    name: 'GuildListSlice',
    initialState: {
        status: 'idle',
        error: null,
        guilds: [],
        activeGuild: null
    } as GuildState,
    reducers: {
        setActiveGuild: (state, action) => {
            return {
                ...state,
                activeGuild: action.payload
            }
        }
    },
    extraReducers: (builder) => {
        builder.addCase(getAllGuildsAsyncThunk.pending, (state) => {
            state.status = 'loading';
            state.error = null;
            state.guilds = [];
        });
        builder.addCase(getAllGuildsAsyncThunk.fulfilled, (state, action: any) => {
            state.error = null;
            state.status = 'complete';
            state.guilds = action.payload;
            state.activeGuild = action.payload[0];
        });
        builder.addCase(getAllGuildsAsyncThunk.rejected, (state, action: any) => {
            state.error = action.error.message;
            state.status = 'failed';
            state.guilds = [];
        });
    }
});

export const IsmSlice = createSlice({
    name: 'IsmListSlice',
    initialState: {
        status: 'idle',
        value: [],
        error: null
    } as IsmState,
    reducers: {},
    extraReducers: (builder) => {
        builder.addCase(getAllSayingsAsyncThunk.pending, (state) => {
            state.status = 'loading';
            state.error = null;
            state.value = [];
        });
        builder.addCase(getAllSayingsAsyncThunk.fulfilled, (state, action: any) => {
            state.error = null;
            state.status = 'complete';
            state.value = action.payload;
        });
        builder.addCase(getAllSayingsAsyncThunk.rejected, (state, action: any) => {
            state.error = action.error.message;
            state.status = 'failed';
            state.value = [];
        });
    }
})

export const { setActiveGuild } = GuildSlice.actions; 