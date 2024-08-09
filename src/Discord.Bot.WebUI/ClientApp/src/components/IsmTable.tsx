import { Box, IconButton } from "@mui/material"
import { DataGrid, GridColDef, GridRowsProp, } from '@mui/x-data-grid'
import Saying from "../data/models/Saying"
import { useState } from "react";
import { useSelector } from 'react-redux';
import EditModal from "./edit-modal/EditModal";
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterMoment } from '@mui/x-date-pickers/AdapterMoment'
import { RootState } from "../data/store/Store";
import { blue } from "@mui/material/colors"
import { AddCircleOutline } from "@mui/icons-material";


export const IsmTable = ({ sayings }: { sayings: Saying[] }) => {

    const [modalOpen, setModalOpen] = useState(false);
    const [activeSaying, setActiveSaying] = useState<Saying | null>(null)
    const activeGuildId = useSelector((state: RootState) => state.Guilds.activeGuild?.id)

    
    const columns: GridColDef[] = [
        { field: 'id', headerName: 'ID' },
        { field: 'ismKey', headerName: 'Key' },
        { field: 'ismSaying', headerName: 'Saying', flex: 1 },
        { field: 'dateCreated', headerName: 'Date Created',flex: 0.5 },
        { field: 'ismRecorder', headerName: 'Recorder' },
    ]


    const openSayingEditModal = (saying: Saying) => {
        setActiveSaying(saying);
        setModalOpen(true);
    }

    const handleModalClose = () => {
        setActiveSaying(null);
        setModalOpen(false);
    }

    return (
        <LocalizationProvider dateAdapter={AdapterMoment}>

            < EditModal
                isModalOpen={modalOpen}
                saying={activeSaying}
                handleClose={handleModalClose}
                ismKeyList={new Set(sayings.map(s => s.ismKey))}
                recorderList={new Set(sayings.map(s => s.ismRecorder))}
                guildId={activeGuildId}
            />

            <IconButton onClick={() => setModalOpen(true)}><AddCircleOutline style={{ color: "white" }} /></IconButton>
            <DataGrid autoHeight rows={sayings} columns={columns} onCellClick={(saying) => openSayingEditModal(saying.row)} />

        </LocalizationProvider>

    )

}


