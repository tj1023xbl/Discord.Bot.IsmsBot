import { Modal } from "@mui/base";
import { AddBoxRounded, ArrowRight, Cancel, Check, CheckBox, DateRange, DeleteOutline, Edit, Publish } from "@mui/icons-material";
import { Autocomplete, Button, Dialog, Icon, IconButton, Popover, TextField, Tooltip, Typography } from "@mui/material";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { AdapterMoment } from "@mui/x-date-pickers/AdapterMoment";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";
import { useEffect, useState } from "react";
import Saying from "../../data/models/Saying";
import styles from './EditModal.module.scss';
import moment from "moment";
import { AppDispatch } from "../../data/store/Store";
import { addNewIsmAPICallAsyncThunk, deleteIsmAPICallAsyncThunk, editIsmAPICallAsyncThunk } from "../../data/store/IsmSlice";

const sx = {
    margin: '10px 0 10px 0'
}

const EditModal = ({ isModalOpen, saying, handleClose, ismKeyList, recorderList, guildId }: { isModalOpen: boolean, saying: Saying | null, handleClose: () => void, ismKeyList: Set<string>, recorderList: Set<string>, guildId: string | undefined }) => {
    const [open, setOpen] = useState(false);
    const initIsm = { guildId: guildId } as Saying
    const [_internalSaying, setInternalSaying] = useState<Saying>(saying ?? initIsm);

    const handleChange = (change: {}) => {
        setInternalSaying(state => {
                return {
                    ...state,
                    ...change
                }
        })
    }

    const handleDelete = () => {
        if (saying) {
            AppDispatch(deleteIsmAPICallAsyncThunk(saying));
            handleClose();
        }
    }

    const handleAdd = () => {
        if (_internalSaying) {
            AppDispatch(addNewIsmAPICallAsyncThunk(_internalSaying));
            handleClose()
        }
    }

    const handleEdit = () => {
        if (_internalSaying) {
            AppDispatch(editIsmAPICallAsyncThunk(_internalSaying));
            handleClose()
        }
    }

    useEffect(() => {
        setInternalSaying(saying ?? initIsm);
        setOpen(isModalOpen);
    }, [saying, isModalOpen])

    return (
        <>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="modal-modal-title"
                aria-describedby="modal-modal-description">

                <section className={styles.container}>
                    <Autocomplete
                        options={Array.from(ismKeyList)}
                        value={_internalSaying?.ismKey ?? ''}
                        freeSolo
                        onChange={(e, value) => handleChange({ ismKey: value })}
                        renderInput={(params) => <TextField {...params} sx={sx} label="Whose ism is it?" />} />
                    <TextField sx={sx} multiline label="Ism Saying" value={_internalSaying?.ismSaying} onChange={(e) => handleChange({ ismSaying: e.target.value })} />
                    <Autocomplete
                        options={Array.from(recorderList)}
                        freeSolo
                        value={_internalSaying?.ismRecorder ?? ''}
                        onChange={(e, value) => handleChange({ ismRecorder: value })}
                        renderInput={(params) => <TextField {...params} sx={sx} label="Who recorded the ism?" />} />
                    <LocalizationProvider dateAdapter={AdapterMoment}>
                        <DatePicker sx={sx} label="When was the ism recorded?" value={_internalSaying ? moment(_internalSaying.dateCreated) : null} onChange={e => handleChange({ dateCreated: e?.format() })} />
                    </LocalizationProvider>
                    <div>
                        {saying ?
                            <Tooltip title="Delete this ism">
                                <IconButton color="error" onClick={handleDelete}><DeleteOutline /></IconButton>
                            </Tooltip>
                            :
                            null
                        }

                        {saying ?
                            <Tooltip title="Submit edits to this ism">
                                <IconButton color="success" onClick={handleEdit}> <Check /> </IconButton>
                            </Tooltip>

                            :

                            <Tooltip title="Add a new saying">
                                <IconButton color="success" onClick={handleAdd}> <Publish /> </IconButton>
                            </Tooltip>
                        }
                        <Tooltip title="Cancel and close this modal">
                            <IconButton color="warning" onClick={handleClose}>< Cancel /></IconButton>
                        </Tooltip>


                    </div>
                </section>

            </Dialog>

        </>
    )

}

export default EditModal;