

/**
 * Represents a saying object.
 */
export default interface Saying {

    /**
     * The primary key for this saying.
     */
    id: string;

    /**
     * The name of the user containing these isms.
     */
    ismKey: string;

    /**
     * The actual ism saying.
     */
    ismSaying: string;

    /**
     * The date and time when the ism was recorded.
     */
    dateCreated: Date;

    /**
     * The username for the person who added this ism record.
     */
    ismRecorder: string;

    /**
     * The ID of the Guild this user belongs to.
     * User isms are unique across guilds.
     */
    guildId: number;
}