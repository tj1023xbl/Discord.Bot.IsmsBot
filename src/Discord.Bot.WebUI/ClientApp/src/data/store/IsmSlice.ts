import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";


interface IsmState {
    activeGuildId: number | null;
}

/**
 * Thunk used to pull all guilds from the db via API
 */
export const getAllGuildsAsyncThunk = createAsyncThunk(
    'thunks/createAsyncThunk',
    async (props) => {
        const getAllGuildsResponse = await fetch('')
    }
)

const IsmSlice = createSlice({
    name: 'IsmSlice',
    initialState: {
        activeGuildId: null
    } as IsmState,
    reducers: {},
    extraReducers: (builder) => {

    }
})